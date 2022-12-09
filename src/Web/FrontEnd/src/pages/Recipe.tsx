import React, { useEffect, useState } from "react";
import {
  IonButton,
  IonButtons,
  IonContent,
  IonHeader,
  IonIcon,
  IonPage,
  IonText,
  IonList,
  IonItem,
  IonLabel,
  IonTitle,
  IonToolbar,
  IonSpinner,
  IonListHeader,
  IonSplitPane,
  IonMenuButton,
  IonInput,
  IonPopover,
  IonCard,
  IonCardSubtitle,
  IonCardTitle,
  IonImg,
  IonGrid,
  IonCol,
  IonRow,
  IonCardContent,
  IonChip,
  IonCardHeader,
  IonTextarea,
  IonBackButton,
  IonBadge,
  IonRange,
  IonItemDivider,
} from "@ionic/react";
import { add, addCircleOutline, arrowBackCircle, backspace, ellipsisHorizontal, ellipsisVertical, heart, heartOutline, personCircle, star, starOutline } from "ionicons/icons";
import { useAuth0 } from "@auth0/auth0-react";
import { useCulina } from "../services/api";
import RecipeViewModel from "../types/view/recipeViewModel";
import LogoutButton from "../components/LogoutButton";
import RecipeSwiper from "../components/RecipeSwiper";
import './Recipe.css';
import StarRating from "../components/StarRating";
import RecipeReviews from "../components/RecipeReviews";
import { V } from "chart.js/dist/chunks/helpers.core";

type ReadRecipePageProps = {
  recipeId: string;
};

const Recipe: React.FC<ReadRecipePageProps> = (props) => {
  const { getAccessTokenSilently } = useAuth0();
  const { getRecipe, favoriteRecipe, unFavoriteRecipe } = useCulina(getAccessTokenSilently);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [recipeNotFound, setRecipeNotFound] = useState<boolean>(false);
  const [recipeViewModel, setRecipeViewModel] = useState<RecipeViewModel | null>(null);

  useEffect(() => {
    setIsLoading(true);
    const loadRecipe = () => {
      getRecipe(props.recipeId)
        .then((vm) => {
          setRecipeViewModel(vm);
          setIsLoading(false);
        })
        .catch(() => {
          setRecipeNotFound(true);
          setIsLoading(false);
        });
    };

    loadRecipe();
  }, [props.recipeId]);

  const onFavoriteButtonClick = () => {
    if (recipeViewModel) {
      const newVm : RecipeViewModel = {
        ...recipeViewModel
      };
      if (!recipeViewModel?.isAFavorite) {
        favoriteRecipe(props.recipeId).then(() => {
          newVm.isAFavorite = true;
          setRecipeViewModel(newVm);
        });
      } else {
        unFavoriteRecipe(props.recipeId).then(() => {
          newVm.isAFavorite = true;
          setRecipeViewModel(newVm);
        });
      }
    }
  };

  return (
    <IonSplitPane contentId="main">
      <IonPage id="main">
      <IonHeader>
        <IonToolbar>
          <IonButtons slot="start">
            <IonBackButton defaultHref="/">Back Button</IonBackButton>
          </IonButtons>
          <IonTitle>Recipe</IonTitle>
          <IonButtons slot="end">
            <IonButton id="profile-button-2">
              <IonIcon slot="icon-only" icon={personCircle}></IonIcon>
            </IonButton>
            <IonPopover trigger="profile-button-2" triggerAction="click">
              {/* <IonContent class="ion-padding">User name</IonContent>
              <IonContent class="ion-padding">Email</IonContent> */}
              <LogoutButton />
            </IonPopover>
          </IonButtons>
          </IonToolbar>
        </IonHeader>
        <IonContent className="page-content" fullscreen>
          <div>
            {isLoading ? (
              <IonSpinner className="page-loader" name="crescent" />
            ) : recipeNotFound ? (
              <IonText>Recipe Not Found</IonText>
            ) : (
              <IonGrid>
                <IonRow class="ion-align-items-start">
                  <IonCol></IonCol>
                  <IonCol size="12" size-md="6" size-lg="5">
                    <IonCard>
                      { !(
                          recipeViewModel &&
                          recipeViewModel.recipe &&
                          recipeViewModel.recipe.images &&
                          recipeViewModel.recipe.images.length >= 1 &&
                          recipeViewModel.recipe.images[0].url)
                        ? (
                          <IonImg
                              src="https://geniuskitchen.sndimg.com/fdc-new/img/fdc-shareGraphic.png"
                              alt="default recipe pic"
                            />
                        )
                        : (
                          <>
                            <IonIcon className="recipe-like-button" title="Favorite" icon={recipeViewModel?.isAFavorite ? heart : heartOutline} onClick={onFavoriteButtonClick} color={recipeViewModel?.isAFavorite ? "danger" : ""} />
                            <IonImg
                                src={recipeViewModel.recipe.images[0].url}
                                alt="recipe pic"
                              />
                          </>
                        )
                      }
                      <IonCardHeader>
                        <IonCardTitle>
                          {recipeViewModel?.recipe.name}
                        </IonCardTitle>
                      </IonCardHeader>
                      <IonCardContent>
                        <IonLabel>
                        { recipeViewModel?.recipe.estimatedMinutes && (<IonBadge style={{ marginRight: 10, padding: 5 }}>{`${recipeViewModel?.recipe.estimatedMinutes} Minutes`}</IonBadge>) }
                        { recipeViewModel?.recipe.serves && (<IonBadge style={{ marginRight: 10, padding: 5 }}>{`Serves ${recipeViewModel?.recipe.serves}`}</IonBadge>) }
                        { recipeViewModel?.recipe.yield && (<IonBadge style={{ marginRight: 10, padding: 5 }}>{`Yields ${recipeViewModel?.recipe.yield}`}</IonBadge>) }
                        { recipeViewModel?.rating && (
                          <IonBadge style={{ marginRight: 10, padding: 5 }}>
                            {`Rating: ${recipeViewModel.rating.toFixed(2)}`}
                          </IonBadge>
                        )}
                        </IonLabel>
                        <IonText>
                          <p style={{ fontSize: 16 }}>{recipeViewModel?.recipe.description}</p>
                        </IonText>
                      </IonCardContent>
                      <IonCardContent>
                        <p>{recipeViewModel?.recipe.tags?.map(x => (<IonChip key={x.tagId}>{ x.tagName }</IonChip>))}</p>
                      </IonCardContent>
                    </IonCard>
                  </IonCol>
                  <IonCol size="12" sizeSm="12" size-md="6" size-lg="5">

                    <IonCard>
                      <IonCardHeader>
                        <IonCardTitle>
                          Ingredients
                        </IonCardTitle>
                      </IonCardHeader>
                      <IonCardContent>
                        <IonList inset={true} lines="inset">
                          { recipeViewModel?.recipe.ingredients.map((i) =>
                            <IonItem key={i.id}>
                              <h2>
                                { i.quantity && (
                                  <span>
                                    {i.quantity}
                                  </span>
                                )}
                                { ` ${i.part}` }
                              </h2>
                              { i.ingredientName && (
                                <IonChip slot="end">
                                  { i.ingredientName }
                                </IonChip>
                              )}
                            </IonItem>
                          )}
                        </IonList>
                      </IonCardContent>
                      <IonCardHeader>
                        <IonCardTitle>
                          Instructions
                        </IonCardTitle>
                      </IonCardHeader>
                      <IonCardContent>
                        <IonList inset={true} lines="inset">
                          { recipeViewModel?.recipe.steps.map((i) =>
                            <IonItem key={i.order}>
                              <h2>
                                { `${i.order}.) ${i.instruction}` }
                              </h2>
                            </IonItem>
                          )}
                        </IonList>
                      </IonCardContent>
                    </IonCard>
                  </IonCol>
                  <IonCol></IonCol>
                </IonRow>
                <IonRow>
                  <IonCol/>
                    <IonCol  size="12" size-md="12" size-lg="10">
                      <RecipeReviews recipeId={props.recipeId} />
                    </IonCol>
                  <IonCol/>
                </IonRow>
                <IonRow>
                  <IonCol/>
                  <IonCol  size="12" size-md="12" size-lg="10">
                    <RecipeSwiper recipeId={props.recipeId}/>
                  </IonCol>
                  <IonCol/>
                </IonRow>
              </IonGrid>
            )}
          </div>
        </IonContent>
      </IonPage>
    </IonSplitPane>
  );
};

export default Recipe;
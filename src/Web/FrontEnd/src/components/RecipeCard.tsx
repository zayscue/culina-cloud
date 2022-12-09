import React, { useState } from "react";
import {
  IonItem,
  IonButtons,
  IonButton,
  IonIcon,
  IonCard,
  IonCardHeader,
  IonCardContent,
  IonCardTitle,
  IonImg,
  IonText,
  IonBadge
} from '@ionic/react';
import { useAuth0 } from "@auth0/auth0-react";
import { useCulina } from "../services/api";
import { heart, heartOutline, reader } from "ionicons/icons";
import RecipeCardViewModel from '../types/view/recipeCardViewModel';
import './RecipeCard.css';

type RecipeCardProps  = {
  vm: RecipeCardViewModel;
};

const RecipeCard: React.FC<RecipeCardProps> = ({ vm }: RecipeCardProps) => {
  const { getAccessTokenSilently } = useAuth0();
  const { favoriteRecipe, unFavoriteRecipe } = useCulina(getAccessTokenSilently);
  const [isAFavorite, setIsAFavorite] = useState<boolean>(vm.isAFavorite);

  const onFavoriteButtonClick = () => {
    if (!isAFavorite) {
      favoriteRecipe(vm.id).then(() => {
        setIsAFavorite(true)
      });
    } else {
      unFavoriteRecipe(vm.id).then(() => {
        setIsAFavorite(false);
      });
    }
  };

  if (vm == null) {
    return (
      <IonCard>
        <IonCardHeader>
          <IonImg
            src="https://geniuskitchen.sndimg.com/fdc-new/img/fdc-shareGraphic.png"
            alt="default recipe pic"
          />
          <IonCardTitle>Recipe</IonCardTitle>
        </IonCardHeader>
        <IonCardContent>Loading...</IonCardContent>
      </IonCard>
    );
  }

  return (
    <IonCard className="recipe-card">
      <IonIcon className="recipe-like-button" title="Favorite" icon={isAFavorite ? heart : heartOutline} onClick={onFavoriteButtonClick} color={isAFavorite ? "danger" : ""} />
      <IonImg className="recipe-photo" src={vm.images[0].url} alt={vm.name} />
      <IonCardContent className="recipe-name">
        <IonItem>
          { vm.rating && (
            <IonBadge slot="start">
              {`Rating: ${vm.rating.toFixed(2)}`}
            </IonBadge>
          )}
          <IonText className="recipe-name-text">
            {vm.name.replace(/(^\w{1})|(\s+\w{1})/g, letter => letter.toUpperCase())}
          </IonText>
          <IonButtons slot="end">
            <IonButton routerLink={`/page/Recipes/${vm.id}`} fill="clear">
              <IonIcon slot="icon-only" ios={reader} md={reader} />
            </IonButton>
          </IonButtons>
        </IonItem>
      </IonCardContent>
    </IonCard>
  );
};

export default RecipeCard;
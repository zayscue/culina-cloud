import React, { useEffect, useRef, useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { useCulina } from "../services/api";
import RecipeReviewViewModel from "../types/view/recipeReviewViewModel";
import {
  IonButton,
  IonCard,
  IonCardContent,
  IonCardHeader,
  IonCardTitle,
  IonChip,
  IonIcon,
  IonItem,
  IonItemDivider,
  IonList,
  IonSkeletonText,
  IonTextarea
} from "@ionic/react";
import StarRating from "./StarRating";
import { add } from "ionicons/icons";
import CreateRecipeReviewCommand from "../types/commands/createRecipeReviewCommand";

type RecipeReviewsComponentProps = {
  recipeId: string;
};

const RecipeReviews: React.FC<RecipeReviewsComponentProps> = (props) => {
  const { getAccessTokenSilently } = useAuth0();
  const { getRecipeReviews, createRecipeReview } = useCulina(getAccessTokenSilently);
  const [rating, setRating] = useState<number>(1);
  const [comment, setComment] = useState<string>();
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [reviews, setReviews] = useState<RecipeReviewViewModel[]>([]);
  const commentInputEl = useRef<HTMLIonTextareaElement>(null);

  useEffect(() => {
    setIsLoading(true);
    getRecipeReviews(props.recipeId, 1, 100).then((respone) => {
      const viewModels : RecipeReviewViewModel[] = respone.items && respone.items.length > 0
        ? respone.items.map((item) => {
          const viewModel : RecipeReviewViewModel = {
            id: item.id,
            rating: item.rating,
            comment: item.comments
          };
          return viewModel;
        }) : [];
        setReviews(viewModels);
        setIsLoading(false);
    });
  }, [props.recipeId]);

  const updateRating = (newRating: number) => {
    setRating(newRating);
  };

  const onPostBtnClick = (e: React.MouseEvent<HTMLIonButtonElement>) => {
    e.preventDefault();
    postRecipeReview({
      recipeId: props.recipeId,
      rating: rating,
      comment: comment
    });
  };

  const postRecipeReview = (newReview : { rating: number, comment?: string, recipeId: string}) =>  {
    const command : CreateRecipeReviewCommand = {
      rating: newReview.rating,
      comments: newReview.comment
    };
    createRecipeReview(newReview.recipeId, command).then((response) => {
      const newViewModel : RecipeReviewViewModel = {
        rating: response.rating,
        id: response.id,
        comment: response.comments
      };
      setReviews([...reviews, newViewModel]);
      setRating(1);
      setComment(undefined);
    })
  };

  return (
    <IonCard>
      <IonCardContent>
        <IonItem lines="inset" >
          <IonChip style={{ marginTop: 10 }} slot="start" color="light">
            <StarRating value={rating} updateRatingCallback={updateRating} />
          </IonChip>
          <IonTextarea ref={commentInputEl} value={comment} onIonInput={(e) => { e.preventDefault(); setComment(e.target?.value ?? undefined ); }} placeholder="Write a review of this recipe" autoGrow={true}></IonTextarea>
        </IonItem>
        <IonItem>
          <IonButton onClick={onPostBtnClick} shape="round" size="default" slot="end">
            <IonIcon slot="start" icon={add}></IonIcon>
            Post
          </IonButton>
        </IonItem>
      </IonCardContent>
      <IonItemDivider title="Reviews"></IonItemDivider>
      <IonCardContent>
        <IonList>
          { isLoading ? (
            <IonItem id="default" lines="none">
              <IonChip slot="start" color="light">
                <StarRating value={0} disabled={true} />
              </IonChip>
              <IonSkeletonText animated={true}></IonSkeletonText>
            </IonItem>
          ): (
            reviews.map((review) => (
              <IonItem key={review.id} lines="none">
                <IonChip slot="start" color="light">
                  <StarRating value={review.rating} disabled={true} />
                </IonChip>
                <IonTextarea disabled={true}>{review.comment}</IonTextarea>
              </IonItem>
            ))
          )}
        </IonList>
      </IonCardContent>
    </IonCard>
  );
};

export default RecipeReviews;

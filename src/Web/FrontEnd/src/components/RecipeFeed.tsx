import React, { useEffect, useState } from "react";
import {
  IonSpinner,
  IonGrid,
  IonCol,
  IonRow,
  IonInfiniteScroll,
  IonInfiniteScrollContent
} from "@ionic/react";
import RecipeCard from './RecipeCard';
import "./RecipeFeed.css";

import RecipeCardViewModel from "../types/view/recipeCardViewModel";
import PaginatedListViewModel from "../types/view/paginatedListViewModel";
import { useAuth0 } from "@auth0/auth0-react";
import { useCulina } from "../services/api";
import { X } from "chart.js/dist/chunks/helpers.core";


declare global {
  interface Array<T> {
    chunk(size: number): Array<Array<T>>;
  }
}

if (!Array.prototype.chunk) {
  // eslint-disable-next-line no-extend-native
  Array.prototype.chunk = function (size: number) {
    const arrayCopy = [...this];
    const results = [];
    while (arrayCopy.length) {
      results.push(arrayCopy.splice(0, size));
    }
    return results;
  };
}

interface RecipeFeedProps {
  recipeFeedName: string;
  startPage: number;
  pageSize: number;
}

type RecipeFeedData = {
  currentPage: number,
  nextPage?: number,
  hasMore?: boolean,
  recipes: Array<Array<RecipeCardViewModel>>
};

const RecipeFeed: React.FC<RecipeFeedProps> = (props) => {
  const { getAccessTokenSilently } = useAuth0();
  const {
    getRecommendedRecipes,
    getFavoriteRecipes,
    getPopularRecipes,
    getRecentRecipes,
    getMyRecipes
  } = useCulina(getAccessTokenSilently);
  const [recipeFeedName, setRecipeFeedName] = useState<string>(props.recipeFeedName);
  const [recipeFeed, setRecipeFeed] = useState<RecipeFeedData>({
    currentPage: props.startPage,
    recipes: []
  });
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const getAPI = (name : string) => {
    switch (name) {
      case 'Recommended':
        return getRecommendedRecipes;
      case 'Favorites':
        return getFavoriteRecipes;
      case 'Recent':
        return getRecentRecipes;
      case 'Popular':
        return getPopularRecipes;
      case 'My_Recipes':
        return getMyRecipes;
      default:
        return getRecommendedRecipes;
    }
  };

  const loadRecipes = (
    name: string,
    limit: number,
    currentState: RecipeFeedData
    ) => {
      const api = getAPI(name);
      setIsLoading(true);
      return api(currentState.currentPage, limit).then(
        (recipePaginatedList) => {
          const viewModel :PaginatedListViewModel<RecipeCardViewModel> = {
            page: recipePaginatedList.page,
            totalCount: recipePaginatedList.totalCount,
            totalPages: recipePaginatedList.totalPages,
            items: recipePaginatedList.items && recipePaginatedList.items.length > 0 ? recipePaginatedList.items.map((x) => ({
              id: x.data.id,
              rating: x.popularity?.ratingWeightedAverage,
              name: x.data.name,
              estimatedMinutes: x.data.estimatedMinutes,
              isAFavorite: x.policy.isAFavorite,
              serves: x.data.serves,
              images: x.data.images
            })) : []
          };
          const newRecipes = viewModel.items.chunk(4);
          const hasMore = viewModel.page < viewModel.totalPages;
          const nextPage = hasMore ? viewModel.page + 1 : undefined;
          setRecipeFeed({
            ...currentState,
            nextPage: nextPage,
            hasMore: hasMore,
            recipes: [...currentState.recipes, ...newRecipes]
          });
          setIsLoading(false);
        }
      );
  }

  useEffect(() => {
    if (recipeFeedName !== props.recipeFeedName) {
      setRecipeFeedName(props.recipeFeedName);
      setRecipeFeed({
        currentPage: props.startPage,
        recipes: []
      });
      setIsLoading(false);
    }
  }, [props.recipeFeedName]);

  useEffect(() => {
    if (!isLoading) {
      loadRecipes(
        recipeFeedName,
        props.pageSize,
        recipeFeed
      );
    }
  }, []);

  if (isLoading && recipeFeed.recipes.length === 0) {
    return (
      <>
        <IonSpinner className="page-loader" name="crescent" />
      </>
    );
  } else {
    return (
      <>
        <IonGrid>
          {recipeFeed.recipes.map((chunk, index) => (
            <IonRow key={`row-${index + 1}`}>
              {chunk.map((vm) => (
                <IonCol
                  key={vm.id}
                  className="ion-align-self-center"
                  size-xl="3"
                  size-lg="3"
                  size-md="6"
                  size-sm="12"
                  size-xs="12"
                >
                  <RecipeCard vm={vm} />
                </IonCol>
              ))}
            </IonRow>
          ))}
        </IonGrid>
        <IonInfiniteScroll
          onIonInfinite={(e: any) => {
            if (!!recipeFeed.nextPage && recipeFeed.hasMore) {
              loadRecipes(
                recipeFeedName,
                props.pageSize,
                {
                  ...recipeFeed,
                  currentPage: recipeFeed.nextPage
                }
              ).then(() => setTimeout(() => e.target.complete(), 500));
            }
          }}
        >
          <IonInfiniteScrollContent></IonInfiniteScrollContent>
        </IonInfiniteScroll>
      </>
    );
  }
};

export default RecipeFeed;

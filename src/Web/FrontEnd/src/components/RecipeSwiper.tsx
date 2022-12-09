import React, { useEffect, useRef, useState } from 'react';
import { IonCard, IonCardHeader, IonContent, IonImg, IonSlide, IonSlides, IonSpinner, IonTitle } from '@ionic/react';
import { Swiper, SwiperSlide } from 'swiper/react';
import { Autoplay, Keyboard, Navigation, Pagination, Scrollbar, Zoom } from 'swiper';
import RecipeCard from './RecipeCard';
import RecipeCardViewModel from '../types/view/recipeCardViewModel';

import 'swiper/swiper.min.css';
import 'swiper/css/navigation';
import 'swiper/css/pagination';
import 'swiper/css/scrollbar';
import '@ionic/react/css/ionic-swiper.css';
import './RecipeSwiper.css';
import { useAuth0 } from '@auth0/auth0-react';
import { useCulina } from '../services/api';

interface RecipeSwiperProps {
  recipeId: string
}

const RecipeSwiper: React.FC<RecipeSwiperProps> = (props)  => {

  const prevEl = useRef('.swiper-button-next');
  const nextEl = useRef('.swiper-button-prev');
  const { getAccessTokenSilently } = useAuth0();
  const { getSimilarRecipes } = useCulina(getAccessTokenSilently);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [similarRecipes, setSimilarRecipes ] = useState<RecipeCardViewModel[]>([]);

  useEffect(() => {
    getSimilarRecipes(props.recipeId, 1, 100).then((response) => {
      const viewModels : RecipeCardViewModel[]  = response.items && response.items.length > 0
        ? response.items.map((item) => {
          const viewModel : RecipeCardViewModel = {
            id: item.data.id,
            rating: item.popularity?.ratingWeightedAverage,
            name: item.data.name,
            estimatedMinutes: item.data.estimatedMinutes,
            isAFavorite: item.policy.isAFavorite,
            serves: item.data.serves,
            images: item.data.images
          };
          return viewModel;
        }) : [];
        setSimilarRecipes(viewModels);
        setIsLoading(false);
    });
  }, [props.recipeId]);


  if (isLoading) {
    return (
      <>
        <IonSpinner name="crescent" />
      </>
    );
  } else {
    if (similarRecipes && similarRecipes.length > 0) {
      return (
        <Swiper
          breakpoints={{
            // when window width is >= 640px
            640: {
              width: 640,
              slidesPerView: 1,
            },
            // when window width is >= 768px
            768: {
              width: 768,
              slidesPerView: 2,
            },
          }}
          autoplay
          loop={true}
          modules={[Autoplay, Keyboard, Zoom]}
          keyboard={true}
          pagination={{ clickable: true }}
          scrollbar={{ draggable: true }}
          zoom={true}
        >
          {similarRecipes.map((similarRecipe) => (
            <SwiperSlide key={similarRecipe.id}>
              <RecipeCard vm={similarRecipe} />
            </SwiperSlide>
          ))}
        </Swiper>
      );
    } else {
      return null;
    }
  }
};

export default RecipeSwiper;
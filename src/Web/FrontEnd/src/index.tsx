import React from 'react';
import { createRoot } from 'react-dom/client';
import App from './App';
import { AUDIENCE, AUTHDOMAIN, CLIENTID } from './constants';
import { Auth0Provider } from '@auth0/auth0-react';
import * as serviceWorkerRegistration from './serviceWorkerRegistration';
import reportWebVitals from './reportWebVitals';
import { BrowserRouter } from 'react-router-dom';

const container = document.getElementById('root');
const root = createRoot(container!);
root.render(
  <Auth0Provider
    domain={AUTHDOMAIN}
    clientId={CLIENTID}
    redirectUri={window.location.origin}
    audience={AUDIENCE}
    cacheLocation="localstorage"
    scope="read:tags read:ingredients create:image read:statistics read:popular_recipes create:recipe read:me read:recommended_recipes read:favorite_recipes read:my_recipes read:recent_recipes read:recipes read:recipe update:recipe create:recipe_nutrition read:recipe_nutrition update:recipe_nutrition read:recipe_steps read:recipe_step update:recipe_steps read:recipe_ingredients create:recipe_ingredient update:recipe_ingredients read:recipe_ingredient read:recipe_images create:recipe_image update:recipe_images read:recipe_image read:recipe_tags create:recipe_tag update:recipe_tags read:recipe_tag read:similar_recipes read:recipe_reviews create:recipe_review create:recipe_favorite delete:recipe_favorite"
  >
    <App />
  </Auth0Provider>
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://cra.link/PWA
serviceWorkerRegistration.unregister();

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();

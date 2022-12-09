import { useAuth0 } from "@auth0/auth0-react";
import { IonApp, IonContent, IonPage, IonRouterOutlet, setupIonicReact } from '@ionic/react';
import { IonReactRouter } from '@ionic/react-router';
import { Redirect, Route } from 'react-router-dom';
import Main from './pages/Main';
import Login from './pages/Login';

/* Core CSS required for Ionic components to work properly */
import '@ionic/react/css/core.css';

/* Basic CSS for apps built with Ionic */
import '@ionic/react/css/normalize.css';
import '@ionic/react/css/structure.css';
import '@ionic/react/css/typography.css';

/* Optional CSS utils that can be commented out */
import '@ionic/react/css/padding.css';
import '@ionic/react/css/float-elements.css';
import '@ionic/react/css/text-alignment.css';
import '@ionic/react/css/text-transformation.css';
import '@ionic/react/css/flex-utils.css';
import '@ionic/react/css/display.css';

/* Theme variables */
import './theme/variables.css';
import Menu from "./components/Menu";
import Recipe from "./pages/Recipe";
import Admin from "./pages/Admin";

setupIonicReact();

const App: React.FC = () => {

  const { isAuthenticated, isLoading } = useAuth0();

  return (
    <IonApp>
      <Menu />
      <IonReactRouter>\
        { isLoading
          ? (
              <IonPage id="main">
                <IonContent className="page-content" fullscreen></IonContent>
              </IonPage>
            )
          : (
            <IonRouterOutlet id="main">
              <Route path="/" exact={true} render={() => {
                return isAuthenticated
                  ? <Redirect to="/page/Recommended" />
                  : <Redirect to="/login" />
              }}/>
              <Route path="/page/:name" exact={true} render={() => {
                return isAuthenticated ? <Main /> : <Redirect to="/login" />;
              }}/>
              <Route exact path="/page/Recipes/:recipeId" render={(props) => {
                return isAuthenticated ? <Recipe recipeId={props.match.params.recipeId}/> : <Redirect to="/login" />;
              }}/>
              <Route exact path="/page/Admin/Statistics"  render={() => {
                return isAuthenticated ? <Admin/> : <Redirect to="/login" />;
              }}/>
              <Route path="/login" exact={true}>
                <Login />
              </Route>
            </IonRouterOutlet>
            )
        }
      </IonReactRouter>


    </IonApp>
  );
};

export default App;

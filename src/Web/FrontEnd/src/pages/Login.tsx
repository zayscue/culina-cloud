import {
  IonButtons,
  IonContent,
  IonHeader,
  IonPage,
  IonToolbar,
  IonImg,
  IonGrid,
  IonRow,
  IonCol,
  IonText
} from '@ionic/react';
import LoginButtonContainer from '../components/LoginButtonContainer';
import './Login.css';

const Login: React.FC = () => {
  return (
    <IonPage>
      <IonHeader>
        <IonToolbar>
          <IonButtons slot="primary">
            <LoginButtonContainer />
          </IonButtons>
        </IonToolbar>
      </IonHeader>
      <IonContent fullscreen className="ion-padding">
        <IonGrid>
          <IonRow class="ion-justify-content-center">
            <IonText>
              <h1>Welcome to Culina!</h1>
            </IonText>
          </IonRow>
          <IonRow class="ion-align-items-center">
            <IonCol />
            <IonCol size="8" size-md="6" size-lg="4">
              <IonImg src="../assets/icon/icon.png" alt="icon-image"></IonImg>
              <IonText>
                <p>
                  Culina offers a robust catalog of over 200k recipes to choose from and explore.
                  To help you find your next favorite recipe, Culina leverages a machine learning recommendation engine powered by a content-based similarity algorithm and a collaborative filtering model informed by user interactions.
                  Once you find a recipe that you like, feel free to favorite it and keep it bookmarked for future reference.
                  You can even rate the recipes using a 1-5 star scale.
                  These reviews will then reinforce the collaborative filtering model, improving future recommendations on the platform.
                </p>
              </IonText>
            </IonCol>
            <IonCol />
          </IonRow>
          <IonRow />
        </IonGrid>
      </IonContent>
    </IonPage>
  );
};

export default Login;
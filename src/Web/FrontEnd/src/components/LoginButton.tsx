import { useAuth0 } from "@auth0/auth0-react";
import { IonButton, IonIcon } from "@ionic/react";
import { logIn } from "ionicons/icons";

const LoginButton = () => {
  const { loginWithPopup } = useAuth0();

  return <IonButton onClick={ () => loginWithPopup() }>
    <IonIcon slot="end" icon={logIn} />
    Log In
  </IonButton>;
};

export default LoginButton;
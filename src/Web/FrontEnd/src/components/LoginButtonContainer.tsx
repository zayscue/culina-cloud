import React, { useEffect } from "react";
import LoginButton from "./LoginButton";
import { useHistory } from 'react-router-dom';
import { useAuth0 } from "@auth0/auth0-react";

interface ContainerProps { }

const LoginButtonContainer: React.FC<ContainerProps> = () => {
  const {isAuthenticated} = useAuth0();
  const history = useHistory();

  useEffect(() => {
    console.log('Reloading LoginContainer...');
    if (isAuthenticated) {
      console.log('Redirecting to "/"...');
      history.push('/');
    }
  }, [isAuthenticated, history]);

  return (
    <LoginButton/>
  );
};

export default LoginButtonContainer;
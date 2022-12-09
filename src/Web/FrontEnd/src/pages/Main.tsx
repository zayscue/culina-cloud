import React, { useEffect, useRef, useState } from 'react';
import {
  IonButton,
  IonButtons,
  IonContent,
  IonHeader,
  IonIcon,
  IonMenuButton,
  IonPage,
  IonPopover,
  IonSearchbar,
  IonSplitPane,
  IonTitle,
  IonToolbar,
  IonModal,
  IonItem,
  IonLabel,
  IonInput,
  IonFab,
  IonFabButton,
  IonFabList,
  IonGrid,
  IonRow
} from '@ionic/react';
import { useHistory, useParams } from 'react-router';
import ExploreContainer from '../components/ExploreContainer';
import Menu from '../components/Menu';
import LogoutButton from '../components/LogoutButton';
import { add, logOut, personCircle, search } from 'ionicons/icons';
import { OverlayEventDetail } from '@ionic/core/components';
import './Main.css';

interface ContainerProps {
};

const Main: React.FC<ContainerProps> = () => {
  const { name } = useParams<{ name: string }>();

  // const modal = useRef<HTMLIonModalElement>(null);
  // const input = useRef<HTMLIonInputElement>(null);

  // const addmodal = useRef<HTMLIonModalElement>(null);
  // const addinput = useRef<HTMLIonInputElement>(null);

  // const [message, setMessage] = useState(
  //   'This modal example uses triggers to automatically open a modal when the button is clicked.'
  // );

  // const [addmessage, setAddMessage] = useState(
  //   'This modal example uses triggers to automatically open a modal when the button is clicked.'
  // );

  // function confirm() {
  //   modal.current?.dismiss(input.current?.value, 'confirm');
  // }

  // function onWillDismiss(ev: CustomEvent<OverlayEventDetail>) {
  //   if (ev.detail.role === 'confirm') {
  //     setMessage(`Hello, ${ev.detail.data}!`);
  //   }
  // }

  // function addConfirm() {
  //   modal.current?.dismiss(input.current?.value, 'confirm');
  // }

  // function addOnWillDismiss(ev: CustomEvent<OverlayEventDetail>) {
  //   if (ev.detail.role === 'confirm') {
  //     setAddMessage(`Hello, ${ev.detail.data}!`);
  //   }
  // }

  return (
    <IonSplitPane contentId="main">
      <IonPage id="main">
        <IonHeader>
          <IonToolbar>
            <IonButtons slot="start">
              <IonMenuButton />
            </IonButtons>
            <IonTitle>{name}</IonTitle>
            <IonButtons slot="end">
              {/* <IonButton id="open-modal" expand="block">
                <IonIcon slot="icon-only" icon={search}></IonIcon>
              </IonButton>
              <IonModal ref={modal} trigger="open-modal" onWillDismiss={(ev) => onWillDismiss(ev)}>
                <IonHeader>
                  <IonToolbar>
                    <IonButtons slot="start">
                      <IonButton onClick={() => modal.current?.dismiss()}>Cancel</IonButton>
                    </IonButtons>
                    <IonTitle>Welcome</IonTitle>
                    <IonButtons slot="end">
                      <IonButton strong={true} onClick={() => confirm()}>
                        Confirm
                      </IonButton>
                    </IonButtons>
                  </IonToolbar>
                </IonHeader>
                <IonContent className="ion-padding">
                  <IonItem>
                    <IonLabel position="stacked">Enter your name</IonLabel>
                    <IonInput ref={input} type="text" placeholder="Your name" />
                  </IonItem>
                </IonContent>
              </IonModal> */}
              <IonButton id="profile-button">
                <IonIcon slot="icon-only" icon={personCircle}></IonIcon>
              </IonButton>
              <IonPopover trigger="profile-button" triggerAction="click">
                {/* <IonContent class="ion-padding">User name</IonContent>
                <IonContent class="ion-padding">Email</IonContent> */}
                <LogoutButton />
              </IonPopover>
            </IonButtons>
          </IonToolbar>
        </IonHeader>
        <IonContent className="page-content" fullscreen>
          {/* <IonHeader collapse="condense">
            <IonToolbar>
              <IonTitle slot="start" size="large">{pageName}</IonTitle>
            </IonToolbar>
          </IonHeader> */}
          <ExploreContainer pageName={name} />
          {/* <div>
            <IonFab slot="fixed" vertical="bottom" horizontal="end">
              <IonFabButton id="open-add-modal">
                <IonIcon icon={add}></IonIcon>
              </IonFabButton>
            </IonFab>
            <IonModal ref={addmodal} trigger="open-add-modal" onWillDismiss={(ev) => onWillDismiss(ev)}>
              <IonHeader>
                <IonToolbar>
                  <IonButtons slot="start">
                    <IonButton onClick={() => addmodal.current?.dismiss()}>Cancel</IonButton>
                  </IonButtons>
                  <IonTitle>Welcome</IonTitle>
                  <IonButtons slot="end">
                    <IonButton strong={true} onClick={() => confirm()}>
                      Confirm
                    </IonButton>
                  </IonButtons>
                </IonToolbar>
              </IonHeader>
              <IonContent className="ion-padding">
                <IonItem>
                  <IonLabel position="stacked">Enter your name</IonLabel>
                  <IonInput ref={addinput} type="text" placeholder="Your name" />
                </IonItem>
              </IonContent>
            </IonModal>
          </div> */}
        </IonContent>
      </IonPage>
    </IonSplitPane>
  );
};

export default Main;

import { useAuth0 } from '@auth0/auth0-react';
import jwt_decode from 'jwt-decode';
import {
  IonContent,
  IonIcon,
  IonItem,
  IonLabel,
  IonList,
  IonListHeader,
  IonMenu,
  IonMenuToggle,
  IonNote,
} from '@ionic/react';

import { sparklesOutline, sparklesSharp, trendingUpOutline, trendingUpSharp, timeOutline, timeSharp, heartOutline, heartSharp, statsChart, statsChartOutline } from 'ionicons/icons';
import { useEffect, useState } from 'react';
import './Menu.css';

interface AppPage {
  url: string;
  iosIcon: string;
  mdIcon: string;
  title: string;
}

const Menu: React.FC = () => {
  const location = window.location;

  const { getAccessTokenSilently, user } = useAuth0();

  const [hasAdminRights, setHasAdminRights] = useState<boolean>(false);

  useEffect(() => {
    getAccessTokenSilently().then((t) => {
      const decoded: { permissions: string[] } = jwt_decode(t);
      const newHasAdminRights = decoded.permissions.includes('read:statistics');
      setHasAdminRights(newHasAdminRights);
    });
  }, []);

  const [appPages] = useState([
    {
      title: 'Recommended',
      url: '/page/Recommended',
      iosIcon: sparklesOutline,
      mdIcon: sparklesSharp
    },
    {
      title: 'Popular',
      url: '/page/Popular',
      iosIcon: trendingUpOutline,
      mdIcon: trendingUpSharp
    },
    {
      title: 'Recent',
      url: '/page/Recent',
      iosIcon: timeOutline,
      mdIcon: timeSharp
    },
    {
      title: 'Favorites',
      url: '/page/Favorites',
      iosIcon: heartOutline,
      mdIcon: heartSharp
    }
  ]);

  return (
    <IonMenu contentId="main" type="overlay">
      <IonContent>
        <IonList id="recipe-feed-list">
          <IonListHeader>Recipes</IonListHeader>
          <IonNote>Explore the different feeds</IonNote>
          {appPages.map((appPage, index) => {
            return (
              <IonMenuToggle key={index} autoHide={false}>
                <IonItem className={location.pathname === appPage.url ? 'selected' : ''} routerLink={appPage.url} lines="none" detail={false}>
                  <IonIcon slot="start" ios={appPage.iosIcon} md={appPage.mdIcon} />
                  <IonLabel>{appPage.title}</IonLabel>
                </IonItem>
              </IonMenuToggle>
            );
          })}
        </IonList>

        {hasAdminRights && (
          <IonList id="admin-function-list">
            <IonListHeader>Admin</IonListHeader>
            <IonMenuToggle autoHide={false}>
              <IonItem className={location.pathname === '/page/Admin/Statistics' ? 'selected' : ''} routerLink="/page/Admin/Statistics" lines="none" detail={false}>
                <IonIcon slot="start" ios={statsChartOutline} md={statsChart} />
                <IonLabel>Statistics</IonLabel>
              </IonItem>
            </IonMenuToggle>
          </IonList>
        )}
      </IonContent>
    </IonMenu>
  );
};

export default Menu;

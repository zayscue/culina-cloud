import {
  IonButtons,
  IonContent,
  IonHeader,
  IonPage,
  IonToolbar,
  IonGrid,
  IonRow,
  IonCol,
  IonText,
  IonSplitPane,
  IonButton,
  IonPopover,
  IonIcon,
  IonTitle,
  IonMenuButton
} from '@ionic/react';
import { Chart as ChartJS, ArcElement, Tooltip, Legend, ChartData, LinearScale, CategoryScale, PointElement, LineElement, ScatterDataPoint } from 'chart.js';
import { Line, Pie } from 'react-chartjs-2';
import { personCircle } from 'ionicons/icons';
import LogoutButton from '../components/LogoutButton';
import { useEffect, useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { useCulina } from '../services/api';

ChartJS.register([CategoryScale, LinearScale, ArcElement, PointElement, LineElement, Tooltip, Legend]);


const randomInteger = (max: number) => {
  return Math.floor(Math.random()*(max + 1));
};

const randomRgbColor = () => {
  const r = randomInteger(255);
  const g = randomInteger(255);
  const b = randomInteger(255);
  return [r,g,b];
};

const randomHexColor = () => {
  const [r,g,b] = randomRgbColor();

  const hr = r.toString(16).padStart(2, '0');
  const hg = g.toString(16).padStart(2, '0');
  const hb = b.toString(16).padStart(2, '0');

  return `#${hr}${hg}${hb}`;
}

type AdminPageViewModel = {
  totalHistoricalRecipes?: number,
  totalHistoricalReviews?: number,
  tagPopularity?: ChartData<"pie", number[], string>
  ingredientPopularity?: ChartData<"pie", number[], string>
  dailyUsers?: ChartData<"line", (number | ScatterDataPoint | null)[], unknown>
};

const Admin: React.FC = () => {
  const [ viewModel, setViewModel ] = useState<AdminPageViewModel>({});

  const { getAccessTokenSilently, user  } = useAuth0();
  const { getApplicationStatistics } = useCulina(getAccessTokenSilently);

  useEffect(() => {
    getApplicationStatistics().then((model) => {
      const tagPieCharViewModel :  ChartData<"pie", number[], string> = {
        labels: model.recipeStatistics.mostPopularTags.slice(0, 25).map(x => x.tagName),
        datasets: [{
          label: '# of Recipes Tagged',
          data: model.recipeStatistics.mostPopularTags.slice(0, 25).map(x => x.totalRecipeTags),
          backgroundColor: model.recipeStatistics.mostPopularTags.slice(0, 25).map(() => randomHexColor())
        }]
      };
      const ingredientPieCharViewModel :  ChartData<"pie", number[], string> = {
        labels: model.recipeStatistics.mostPopularIngredients.slice(0, 25).map(x => x.ingredientName),
        datasets: [{
          label: '# of Ingredient References',
          data: model.recipeStatistics.mostPopularIngredients.slice(0, 25).map(x => x.totalIngredientReferences),
          backgroundColor: model.recipeStatistics.mostPopularIngredients.slice(0, 25).map(() => randomHexColor())
        }]
      };
      const dailyUsersLineChartViewModel : ChartData<"line", (number | ScatterDataPoint | null)[], unknown> = {
        labels: model.userStatistics.dailyApplicationUsersStatistics.slice(0, 30).reverse().map(x => x.date.toString().split('T')[0]),
        datasets: [{
          label: 'Logins',
          data: model.userStatistics.dailyApplicationUsersStatistics.slice(0, 30).reverse().map(x => x.logins),
          fill: false,
          borderColor: randomHexColor(),
          tension: 0.1
        }, {
          label: 'SignUps',
          data: model.userStatistics.dailyApplicationUsersStatistics.slice(0, 30).reverse().map(x => x.signUps),
          fill: false,
          borderColor: randomHexColor(),
          tension: 0.1
        }]
      };
      setViewModel({
        totalHistoricalRecipes: model.recipePopularityStatistics.totalHistoricalRecipes,
        totalHistoricalReviews: model.recipePopularityStatistics.totalHistoricalReviews,
        tagPopularity: tagPieCharViewModel,
        ingredientPopularity: ingredientPieCharViewModel,
        dailyUsers: dailyUsersLineChartViewModel
      });
    });
  }, []);

  return (
    <IonSplitPane contentId="main">
      <IonPage id="main">
      <IonHeader>
        <IonToolbar>
          <IonButtons slot="start">
            <IonMenuButton />
          </IonButtons>
          <IonTitle>Statistics</IonTitle>
          <IonButtons slot="end">
            <IonButton id="admin-profile-button">
              <IonIcon slot="icon-only" icon={personCircle}></IonIcon>
            </IonButton>
            <IonPopover trigger="admin-profile-button" triggerAction="click">
              <LogoutButton />
            </IonPopover>
          </IonButtons>
          </IonToolbar>
        </IonHeader>
      <IonContent fullscreen className="ion-padding">
        <IonGrid>
          <IonRow class="ion-justify-content-center">
            <IonCol size="8" size-md="8" size-lg="4">
              { viewModel.totalHistoricalRecipes && (
                <IonText>
                  <h1>Total Recipes: {viewModel.totalHistoricalRecipes}</h1>
                </IonText>
              )}
              { viewModel.totalHistoricalRecipes && (
                <IonText>
                  <h1>Total Reviews: {viewModel.totalHistoricalReviews}</h1>
                </IonText>
              )}
            </IonCol>
            <IonCol size="8" size-md="8" size-lg="4">
              <IonText>
                <h1>Tag Popularity</h1>
              </IonText>
              { viewModel.tagPopularity && (
                <Pie title='Tag Popularity' data={viewModel.tagPopularity} />
              )}
            </IonCol>
            <IonCol size="8" size-md="8" size-lg="4">
              <IonText>
                <h1>Ingredient Popularity</h1>
              </IonText>
              { viewModel.ingredientPopularity && (
                <Pie title='Ingredient Popularity'  data={viewModel.ingredientPopularity} />
              )}
            </IonCol>
          </IonRow>
          <IonRow class="ion-justify-content-center">
            <IonCol>
              <IonText>
                <h1>Daily Users</h1>
              </IonText>
              { viewModel.dailyUsers && (
                <Line title='Daily Users' data={viewModel.dailyUsers} />
              )}
            </IonCol>
          </IonRow>
        </IonGrid>
      </IonContent>
    </IonPage>
    </IonSplitPane>
  );
};

export default Admin;
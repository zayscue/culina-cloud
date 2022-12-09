import { useAuth0 } from '@auth0/auth0-react';
import { useCulina } from '../services/api';
import RecipeFeed from "./RecipeFeed";
import './ExploreContainer.css';
import { useEffect, useState } from 'react';

interface ContainerProps {
  pageName: string;
}

const ExploreContainer: React.FC<ContainerProps> = ({ pageName }) => {
  return (
    <div className="container">
      <RecipeFeed recipeFeedName={pageName} startPage={1} pageSize={100} />
    </div>
  );
};

export default ExploreContainer;

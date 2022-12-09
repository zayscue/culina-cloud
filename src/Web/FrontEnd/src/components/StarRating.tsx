import { useState } from "react";

import './StarRating.css';

const Star: React.FC<{ marked: boolean, starId: number }> = ({ marked, starId }) => {
  return (
    <span data-star-id={starId} className="star" role="button">
      {marked ? '\u2605' : '\u2606'}
    </span>
  );
};

const StarRating: React.FC<{ value: number, updateRatingCallback? : (newRating: number) => void,  disabled?: boolean }> = ({ value, updateRatingCallback, disabled = false }) => {
  const [selection, setSelection] = useState(value);

  const hoverOver = (e: React.MouseEvent<HTMLDivElement, MouseEvent>) => {
    if (!disabled) {
      let val = 0;
      if (e && e.target) {
        const el = e.target as HTMLInputElement;
        if (el && el.getAttribute('data-star-id')) {
          const attr = el.getAttribute('data-star-id') ?? "0";
          val = parseInt(attr, 10);
        }
      }
      setSelection(val);
    }
  };
  return (
    <div
      onMouseOut={(e) => {
        if (!disabled) {
          setSelection(value);
        }
      }}
      onClick={(e: React.MouseEvent<HTMLDivElement, MouseEvent>) => {
        if (!disabled) {
          const el = e.target as HTMLInputElement;
          const attr = el.getAttribute('data-star-id') ?? "0";
          const s = parseInt(attr, 10);
          // setRating(s || rating);
          if (updateRatingCallback) {
            updateRatingCallback(s || value);
          }
        }
      }}
      onMouseOver={hoverOver}
    >
      {Array.from({ length: 5 }, (v, i) => (
        <Star
          starId={i + 1}
          key={`star_${i + 1}`}
          marked={selection ? selection >= i + 1 : value >= i + 1}
        />
      ))}
    </div>
  );
};

export default StarRating;

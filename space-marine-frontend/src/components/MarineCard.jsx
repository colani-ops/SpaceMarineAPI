import { Link } from 'react-router-dom';

export default function MarineCard({ marine }) {
  return (
    <div style={{ border: '1px solid #ccc', padding: '10px', margin: '10px' }}>
      <h4>
        <Link to={`/marine/${marine.id}`}>
          {marine.firstName} {marine.lastName}
        </Link>
      </h4>
      <p>Age: {marine.age}</p>
      <p>Experience: {marine.experience}</p>
    </div>
  );
}

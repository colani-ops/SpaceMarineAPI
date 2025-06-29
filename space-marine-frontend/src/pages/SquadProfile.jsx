import { useParams, Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import axios from 'axios';

export default function SquadProfile() {
  const { id } = useParams();
  const [squad, setSquad] = useState(null);
  const [marines, setMarines] = useState([]);

  useEffect(() => {
    axios.get(`https://localhost:7170/api/Squad/${id}`)
      .then(response => setSquad(response.data))
      .catch(err => console.error('Failed to fetch squad:', err));

    axios.get(`https://localhost:7170/api/SpaceMarine/bysquad/${id}`)
      .then(response => setMarines(response.data))
      .catch(err => console.error('Failed to fetch marines:', err));
  }, [id]);

  if (!squad) return <p>Loading squad...</p>;

  return (
    <div style={{ padding: '1rem' }}>
      {squad.portraitImage && (
        <img
          src={`/images/${squad.portraitImage}`}
          alt="Squad Portrait"
          style={{
            width: "250px",
            height: "250px",
            objectFit: "cover",
            marginBottom: "1rem"
          }}
        />
      )}
      <h2>{squad.name}</h2>
      <p><strong>Type:</strong> {squad.type}</p>

      <h3>Marines in this squad:</h3>
      {marines.length > 0 ? (
        <ul>
          {marines.map(m => (
            <li key={m.id} style={{ display: "flex", alignItems: "center", gap: "0.5rem" }}>
              {m.portraitImage && (
                <img
                  src={`/images/${m.portraitImage}`}
                  alt="Marine Portrait"
                  style={{
                    width: "40px",
                    height: "40px",
                    objectFit: "cover",
                    borderRadius: "4px"
                  }}
                />
              )}
              <Link to={`/marine/${m.id}`}>
                {m.firstName} {m.lastName} (Age: {m.age})
              </Link>
            </li>
          ))}
        </ul>
      ) : (
        <p>No marines found in this squad.</p>
      )}
    </div>
  );
}

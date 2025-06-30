import { useParams, Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import axios from 'axios';

export default function SquadProfile() {
  const { id } = useParams();
  const [squad, setSquad] = useState(null);
  const [users, setUsers] = useState([]);

  useEffect(() => {
    axios.get(`https://localhost:7170/api/Squad/${id}`)
      .then(response => setSquad(response.data))
      .catch(err => console.error('Failed to fetch squad:', err));

    axios.get(`https://localhost:7170/api/User/bysquad/${id}`)
      .then(response => setUsers(response.data))
      .catch(err => console.error('Failed to fetch users:', err));
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

      <h3>Members in this squad:</h3>
      {users.length > 0 ? (
        <ul>
          {users.map(u => (
            <li key={u.id} style={{ display: "flex", alignItems: "center", gap: "0.5rem" }}>
              {u.portraitImage && (
                <img
                  src={`https://localhost:7170/images/${u.portraitImage}`}
                  alt="Portrait"
                  style={{
                    width: "40px",
                    height: "40px",
                    objectFit: "cover",
                    borderRadius: "4px"
                  }}
                />
              )}
              <Link to={`/user/${u.id}`}>
                {u.displayName || u.username}
              </Link>
            </li>
          ))}
        </ul>
      ) : (
        <p>No users found in this squad.</p>
      )}
    </div>
  );
}

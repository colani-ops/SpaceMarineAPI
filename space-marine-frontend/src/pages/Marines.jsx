import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';

export default function Marines() {
  const [marines, setMarines] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    axios.get('https://localhost:7170/api/SpaceMarine')
      .then(res => {
        setMarines(res.data);
        setLoading(false);
      })
      .catch(err => {
        console.error('Failed to fetch marines:', err);
        setLoading(false);
      });
  }, []);

  if (loading) return <p>Loading marines...</p>;

  return (
    <div style={{ padding: '1rem' }}>
      <h2>All Marines</h2>
      {marines.length === 0 ? (
        <p>No marines found.</p>
      ) : (
        <ul>
  {marines.map(m => (
    <li key={m.id} style={{ display: "flex", alignItems: "center", gap: "0.5rem" }}>
      {m.portraitImage && (
        <img
          src={`/images/${m.portraitImage}`}
          alt="Portrait"
          style={{
            width: "30px",
            height: "30px",
            objectFit: "cover",
            borderRadius: "4px"
          }}
        />
      )}
      <Link to={`/marine/${m.id}`}>
        {m.firstName} {m.lastName}
      </Link>
      (Age: {m.age}, XP: {m.experience}) —{" "}
      <Link to={`/squad/${m.squadId}`}>
        Squad {m.squadId}
      </Link>
    </li>
  ))}
</ul>

      )}
    </div>
  );
}

// src/pages/MarineProfile.jsx

import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import axios from 'axios';

export default function MarineProfile() {
  const { id } = useParams();
  const [marine, setMarine] = useState(null);
  const [squad, setSquad] = useState(null);

  useEffect(() => {
    axios.get(`https://localhost:7170/api/SpaceMarine/${id}`)
      .then(res => {
        setMarine(res.data);
        console.log("Marine:", res.data); // Debugging aid

        if (res.data.squadId) {
          return axios.get(`https://localhost:7170/api/Squad/${res.data.squadId}`);
        }
      })
      .then(res => {
        if (res) setSquad(res.data);
      })
      .catch(err => console.error("Error loading marine:", err));
  }, [id]);

  if (!marine) return <p>Loading marine...</p>;

  return (
    <div style={{ padding: '1rem' }}>

      {marine.portraitImage && (
      <img
        src={`/images/${marine.portraitImage}`}
        alt="Marine Portrait"
        style={{ width: "200px", height: "200px", objectFit: "cover", marginBottom: "1rem" }}
      />
)}


      <h2>{marine.firstName} {marine.lastName}</h2>
      <p>Age: {marine.age}</p>
      <p>Experience: {marine.experience}</p>
      {squad && (
        <p>Squad: <Link to={`/squad/${squad.id}`}>{squad.name}</Link> ({squad.type})</p>
      )}
    </div>
  );
}

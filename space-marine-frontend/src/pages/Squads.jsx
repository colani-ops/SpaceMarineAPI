// src/pages/Squads.jsx

import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';

export default function Squads() {
  const [squads, setSquads] = useState([]);
  const [expandedSquadId, setExpandedSquadId] = useState(null);
  const [marinesBySquad, setMarinesBySquad] = useState({});

  useEffect(() => {
    axios.get('https://localhost:7170/api/Squad')
      .then(response => setSquads(response.data))
      .catch(error => console.error("Failed to fetch squads:", error));
  }, []);

  const toggleSquad = (id) => {
    const isOpen = expandedSquadId === id;
    setExpandedSquadId(isOpen ? null : id);

    if (!isOpen && !marinesBySquad[id]) {
      axios.get(`https://localhost:7170/api/SpaceMarine/bysquad/${id}`)
        .then(res => {
          setMarinesBySquad(prev => ({ ...prev, [id]: res.data }));
        })
        .catch(err => console.error(`Failed to fetch marines for squad ${id}:`, err));
    }
  };

  return (
    <div>
      <h2>All Squads</h2>
      {squads.map(squad => (
        <div key={squad.id} style={{ border: '1px solid gray', marginBottom: '10px', padding: '10px' }}>
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <Link to={`/squad/${squad.id}`} style={{ fontWeight: 'bold' }}>
              {squad.name} ({squad.type})
            </Link>
            <button onClick={() => toggleSquad(squad.id)}>
              {expandedSquadId === squad.id ? 'Hide Marines' : 'Show Marines'}
            </button>
          </div>

          {expandedSquadId === squad.id && marinesBySquad[squad.id] && (
            <ul>
              {marinesBySquad[squad.id].map(m => (
                <li key={m.id}>
                  <Link to={`/marine/${m.id}`}>{m.firstName} {m.lastName}</Link>
                </li>
              ))}
            </ul>
          )}
        </div>
      ))}
    </div>
  );
}

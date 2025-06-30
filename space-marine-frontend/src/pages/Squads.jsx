import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';

export default function Squads() {
  const [squads, setSquads] = useState([]);
  const [expandedSquadId, setExpandedSquadId] = useState(null);
  const [usersBySquad, setUsersBySquad] = useState({});

  useEffect(() => {
    axios.get('https://localhost:7170/api/Squad')
      .then(response => setSquads(response.data))
      .catch(error => console.error("Failed to fetch squads:", error));
  }, []);

  const toggleSquad = (id) => {
    const isOpen = expandedSquadId === id;
    setExpandedSquadId(isOpen ? null : id);

    if (!isOpen && !usersBySquad[id]) {
      axios.get(`https://localhost:7170/api/User/bysquad/${id}`)
        .then(res => {
          setUsersBySquad(prev => ({ ...prev, [id]: res.data }));
        })
        .catch(err => console.error(`Failed to fetch users for squad ${id}:`, err));
    }
  };

  return (
    <div>
      <h2>All Squads</h2>
      {squads.map(squad => (
        <div
          key={squad.id}
          style={{
            display: "flex",
            alignItems: "center",
            border: "1px solid gray",
            marginBottom: "10px",
            padding: "10px"
          }}
        >
          {squad.portraitImage && (
            <img
              src={`/images/${squad.portraitImage}`}
              alt="Squad portrait"
              style={{
                width: "50px",
                height: "50px",
                objectFit: "cover",
                marginRight: "1rem"
              }}
            />
          )}
          <div style={{ flex: 1 }}>
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
              <Link to={`/squad/${squad.id}`} style={{ fontWeight: "bold" }}>
                {squad.name} ({squad.type})
              </Link>
              <button onClick={() => toggleSquad(squad.id)}>
                {expandedSquadId === squad.id ? "Hide Members" : "Show Members"}
              </button>
            </div>
            {expandedSquadId === squad.id && usersBySquad[squad.id] && (
              <ul>
                {usersBySquad[squad.id].map((u) => (
                  <li
                    key={u.id}
                    style={{
                      display: "flex",
                      alignItems: "center",
                      marginBottom: "0.25rem"
                    }}
                  >
                    {u.portraitImage && (
                      <img
                        src={`https://localhost:7170/images/${u.portraitImage}`}
                        alt="Portrait"
                        style={{
                          width: "30px",
                          height: "30px",
                          marginRight: "0.5rem",
                          objectFit: "cover",
                          borderRadius: "3px"
                        }}
                      />
                    )}
                    <Link to={`/user/${u.id}`}>
                      {u.displayName || u.username}
                    </Link>
                  </li>
                ))}
              </ul>
            )}
          </div>
        </div>
      ))}
    </div>
  );
}

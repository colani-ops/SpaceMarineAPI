import { useEffect, useState } from "react";
import axios from "axios";

export default function ManageSquads() {
  const [squads, setSquads] = useState([]);
  const [newSquad, setNewSquad] = useState({ name: "", type: "Tactical" });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchSquads();
  }, []);

  const fetchSquads = () => {
    setLoading(true);
    axios
      .get("https://localhost:7170/api/Squad")
      .then((res) => setSquads(res.data))
      .catch((err) => console.error("Error fetching squads", err))
      .finally(() => setLoading(false));
  };

  const createSquad = () => {
    axios
      .post("https://localhost:7170/api/Squad", newSquad)
      .then(() => {
        setNewSquad({ name: "", type: "Tactical" });
        fetchSquads();
      })
      .catch((err) => console.error("Error creating squad", err));
  };

  const deleteSquad = (id) => {
    if (!window.confirm("Are you sure you want to delete this squad?")) return;
    axios
      .delete(`https://localhost:7170/api/Squad/${id}`)
      .then(fetchSquads)
      .catch((err) => console.error("Error deleting squad", err));
  };

  return (
    <div style={{ padding: "2rem" }}>
      <h2>Manage Squads</h2>

      <div style={{ marginBottom: "1rem" }}>
        <input
          placeholder="Squad Name"
          value={newSquad.name}
          onChange={(e) => setNewSquad({ ...newSquad, name: e.target.value })}
        />
        <select
          value={newSquad.type}
          onChange={(e) => setNewSquad({ ...newSquad, type: e.target.value })}
        >
          <option value="Tactical">Tactical</option>
          <option value="Assault">Assault</option>
          <option value="Recon">Recon</option>
        </select>
        <button onClick={createSquad}>Create Squad</button>
      </div>

      {loading ? (
        <p>Loading squads...</p>
      ) : (
        <ul>
          {squads.map((s) => (
            <li key={s.id} style={{ marginBottom: "0.5rem" }}>
              <strong>{s.name}</strong> ({s.type})
              <button
                style={{ marginLeft: "1rem" }}
                onClick={() => deleteSquad(s.id)}
              >
                Delete
              </button>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

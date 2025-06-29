import { useEffect, useState } from "react";
import axios from "axios";

export default function ManageSquads() {
  const [squads, setSquads] = useState([]);
  const [newSquad, setNewSquad] = useState({ name: "", type: "Tactical", portraitImage: "" });
  const [editingId, setEditingId] = useState(null);
  const [editValues, setEditValues] = useState({ name: "", type: "" });
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

  const startEditing = (squad) => {
    setEditingId(squad.id);
    setEditValues({ 
      name: squad.name, 
      type: squad.type,
      portraitImage: squad.portraitImage || ""
     });
  };

  const cancelEditing = () => {
    setEditingId(null);
    setEditValues({ name: "", type: "", portraitImage: "" });
  };

  const saveEdit = (id) => {
    axios
      .put(`https://localhost:7170/api/Squad/${id}`, editValues)
      .then(() => {
        cancelEditing();
        fetchSquads();
      })
      .catch((err) => console.error("Error updating squad", err));
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
            <li key={s.id} style={{ display: "flex", alignItems: "center", marginBottom: "0.5rem" }}>
          {s.portraitImage && (
            <img
              src={`/images/${s.portraitImage}`}
              alt="Squad"
              style={{ width: "50px", height: "50px", objectFit: "cover", marginRight: "1rem" }}
            />
          )}
          {editingId === s.id ? (
            <>
            <select
              value={editValues.portraitImage || "defaultSquad.png"}
                onChange={(e) => setEditValues({ ...editValues, portraitImage: e.target.value })}
            >
              <option value="squadDefault.png">Default</option>
              <option value="squadAssault.png">Assault</option>
              <option value="squadCommand.png">Command</option>
              <option value="squadTactical.png">Tactical</option>
              <option value="squadVeteran.png">Veteran</option>
              </select>

            <input
              placeholder="Name"
              value={editValues.name}
              onChange={(e) => setEditValues({ ...editValues, name: e.target.value })}
            />
            <select
              value={editValues.type}
              onChange={(e) => setEditValues({ ...editValues, type: e.target.value })}
            >
              <option value="Tactical">Tactical</option>
              <option value="Assault">Assault</option>
              <option value="Recon">Recon</option>
            </select>
          <button onClick={() => saveEdit(s.id)}>Save</button>
          <button onClick={cancelEditing}>Cancel</button>
        </>

        ) : (

        <>
          <strong>{s.name}</strong> ({s.type})
            <button style={{ marginLeft: "1rem" }} onClick={() => startEditing(s)}>Edit</button>
            <button style={{ marginLeft: "0.5rem" }} onClick={() => deleteSquad(s.id)}>Delete</button>
        </>
      )}

      </li>

        ))}

      </ul>

      )}

    </div>
  );
}

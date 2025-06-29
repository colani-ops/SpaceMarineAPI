import { useEffect, useState } from "react";
import axios from "axios";

export default function ManageMarines() {
  const [marines, setMarines] = useState([]);
  const [squads, setSquads] = useState([]);
  const [loading, setLoading] = useState(true);

  const [selectedFile, setSelectedFile] = useState(null);


  const [newMarine, setNewMarine] = useState({
    firstName: "",
    lastName: "",
    age: "",
    experience: "",
    squadId: ""
  });

  const [editingId, setEditingId] = useState(null);
  const [editingMarine, setEditingMarine] = useState({
    firstName: "",
    lastName: "",
    age: "",
    experience: "",
    squadId: ""
  });

  useEffect(() => {
    fetchData();
  }, []);

  function fetchData() {
    setLoading(true);
    Promise.all([
      axios.get("https://localhost:7170/api/SpaceMarine"),
      axios.get("https://localhost:7170/api/Squad")
    ])
      .then(([marinesRes, squadsRes]) => {
        setMarines(marinesRes.data);
        setSquads(squadsRes.data);
      })
      .catch(err => console.error("Error fetching data:", err))
      .finally(() => setLoading(false));
  }

  function handleCreate() {
    if (!newMarine.firstName || !newMarine.lastName) {
      alert("First and last name are required.");
      return;
    }
    axios
      .post("https://localhost:7170/api/SpaceMarine", newMarine)
      .then(() => {
        setNewMarine({
          firstName: "",
          lastName: "",
          age: "",
          experience: "",
          squadId: ""
        });
        fetchData();
      })
      .catch(err => console.error("Error creating marine:", err));
  }

  function startEdit(m) {
    setEditingId(m.id);
    setEditingMarine({
      firstName: m.firstName,
      lastName: m.lastName,
      age: m.age,
      experience: m.experience,
      squadId: m.squadId
    });
  }

  async function handleSaveEdit(id) {
  try {
    let portraitFilename = editingMarine.portraitImage || "";

    // If user selected a new file, upload it first
    if (selectedFile) {
      const formData = new FormData();
      formData.append("file", selectedFile);

      const uploadResponse = await axios.post("https://localhost:7170/api/upload", formData);
      portraitFilename = uploadResponse.data.filename;
    }

    // Now PUT the updated marine with the new portrait filename
    await axios.put(`https://localhost:7170/api/SpaceMarine/${id}`, {
      ...editingMarine,
      portraitImage: portraitFilename
    });

    setEditingId(null);
    setSelectedFile(null); // clear the file
    fetchData();
  } catch (err) {
    console.error("Error updating marine:", err);
  }
}


  function handleDelete(id) {
    if (window.confirm("Delete this marine?")) {
      axios
        .delete(`https://localhost:7170/api/SpaceMarine/${id}`)
        .then(() => fetchData())
        .catch(err => console.error("Error deleting marine:", err));
    }
  }

  return (
    <div style={{ padding: "1rem" }}>
      <h2>Manage Marines</h2>

      <h3>Create New Marine</h3>
      <div style={{ marginBottom: "1rem" }}>
        <input
          placeholder="First name"
          value={newMarine.firstName}
          onChange={(e) =>
            setNewMarine({ ...newMarine, firstName: e.target.value })
          }
        />
        <input
          placeholder="Last name"
          value={newMarine.lastName}
          onChange={(e) =>
            setNewMarine({ ...newMarine, lastName: e.target.value })
          }
        />
        <input
          type="number"
          placeholder="Age"
          value={newMarine.age}
          onChange={(e) =>
            setNewMarine({ ...newMarine, age: e.target.value })
          }
        />
        <input
          type="number"
          placeholder="Experience"
          value={newMarine.experience}
          onChange={(e) =>
            setNewMarine({ ...newMarine, experience: e.target.value })
          }
        />
        <select
          value={newMarine.squadId}
          onChange={(e) =>
            setNewMarine({ ...newMarine, squadId: e.target.value })
          }
        >
          <option value="">Select squad</option>
          {squads.map((s) => (
            <option key={s.id} value={s.id}>
              {s.name}
            </option>
          ))}
        </select>
        <button onClick={handleCreate}>Create Marine</button>
      </div>

      {loading ? (
        <p>Loading marines...</p>
      ) : (
        marines.map((m) => (
          <div
            key={m.id}
            style={{
              border: "1px solid #ccc",
              marginBottom: "0.5rem",
              padding: "0.5rem"
            }}
          >
            {editingId === m.id ? (
              <>
                
                <input 
                type="file" 
                onChange={(e) => setSelectedFile(e.target.files[0])} 
                />
                {m.portraitImage && (

                  console.log("Marine image:", m.portraitImage),

                  <img
                    src={`https://localhost:7170/images/${marine.portraitImage}`}
                    alt="Portrait"
                    style={{
                      width: "50px",
                      height: "50px",
                      objectFit: "cover",
                      marginLeft: "0.5rem"
                    }}
                  />
                )}

                <input
                  value={editingMarine.firstName}
                  onChange={(e) =>
                    setEditingMarine({ ...editingMarine, firstName: e.target.value })
                  }
                />
                
                <input
                  value={editingMarine.lastName}
                  onChange={(e) =>
                    setEditingMarine({ ...editingMarine, lastName: e.target.value })
                  }
                />
                
                <input
                  type="number"
                  value={editingMarine.age}
                  onChange={(e) =>
                    setEditingMarine({ ...editingMarine, age: e.target.value })
                  }
                />
                
                <input
                  type="number"
                  value={editingMarine.experience}
                  onChange={(e) =>
                    setEditingMarine({ ...editingMarine, experience: e.target.value })
                  }
                />
                
                <select
                  value={editingMarine.squadId}
                  onChange={(e) =>
                    setEditingMarine({ ...editingMarine, squadId: e.target.value })
                  }
                >
                
                <option value="">Select squad</option>
                  {squads.map((s) => (
                    <option key={s.id} value={s.id}>
                      {s.name}
                    </option>
                  ))}
                </select>
                
                <button onClick={() => handleSaveEdit(m.id)}>Save</button>
                
                <button onClick={() => setEditingId(null)}>Cancel</button>
              
              </>
            
          ) : (
              <>
                <strong>{m.firstName} {m.lastName}</strong> (Age: {m.age}, XP: {m.experience})
                — Squad: {squads.find(s => s.id === m.squadId)?.name || "Unassigned"}
                <button onClick={() => startEdit(m)}>Edit</button>
                <button onClick={() => handleDelete(m.id)}>Delete</button>
              </>
            )}
          </div>
        ))
      )}
    </div>
  );
}

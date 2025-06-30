import { useEffect, useState } from "react";
import axios from "axios";
import { useAuth } from "../AuthContext";

export default function ManageUsers() {
  const { user } = useAuth();

  const [users, setUsers] = useState([]);
  const [squads, setSquads] = useState([]);
  const [loading, setLoading] = useState(true);

  const [editingId, setEditingId] = useState(null);
  const [editingUser, setEditingUser] = useState({});
  const [selectedFile, setSelectedFile] = useState(null);

  useEffect(() => {
    fetchData();
  }, []);

  function fetchData() {
    setLoading(true);
    Promise.all([
      axios.get("https://localhost:7170/api/User"),
      axios.get("https://localhost:7170/api/Squad")
    ])
      .then(([usersRes, squadsRes]) => {
        setUsers(usersRes.data);
        setSquads(squadsRes.data);
      })
      .catch(err => console.error("Error fetching data:", err))
      .finally(() => setLoading(false));
  }

  function startEdit(u) {
    setEditingId(u.id);
    setEditingUser({
      displayName: u.displayName || "",
      firstName: u.firstName || "",
      lastName: u.lastName || "",
      age: u.age || "",
      experience: u.experience || "",
      portraitImage: u.portraitImage || ""
    });
    setSelectedFile(null);
  }

  async function saveProfile(id) {
    let portraitFilename = editingUser.portraitImage;

    if (selectedFile) {
      const formData = new FormData();
      formData.append("file", selectedFile);

      const uploadResponse = await axios.post(
        "https://localhost:7170/api/upload",
        formData
      );
      portraitFilename = uploadResponse.data.filename;
    }

    await axios.put(`https://localhost:7170/api/User/${id}/profile`, {
      ...editingUser,
      portraitImage: portraitFilename
    });

    setEditingId(null);
    setSelectedFile(null);
    fetchData();
  }

  function handleRoleChange(userId, newRole) {
    axios
      .put(`https://localhost:7170/api/User/${userId}/role`, { role: newRole })
      .then(fetchData)
      .catch(err => console.error("Error updating role:", err));
  }

  function handleSquadChange(userId, squadId) {
    const parsed = squadId === "" ? null : parseInt(squadId, 10);
    axios
      .put(`https://localhost:7170/api/User/${userId}/squad`, { squadId: parsed })
      .then(fetchData)
      .catch(err => console.error("Error updating squad:", err));
  }

  function handleDelete(userId) {
    if (window.confirm("Delete this user?")) {
      axios
        .delete(`https://localhost:7170/api/User/${userId}`)
        .then(fetchData)
        .catch(err => console.error("Error deleting user:", err));
    }
  }

  if (loading) return <p>Loading users...</p>;

  return (
    <div style={{ padding: "1rem" }}>
      <h2>Manage Users</h2>
      <ul>
        {users.map(u => (
          <li key={u.id} style={{ marginBottom: "1rem" }}>
            <strong>{u.username}</strong> ({u.role}){" "}
            {u.role === "SuperAdmin" ? (
              <span style={{ marginLeft: "1rem", fontStyle: "italic", color: "gray" }}>
                (Cannot edit or delete)
              </span>
            ) : (
              <>
                <select
                  value={u.role}
                  onChange={(e) => handleRoleChange(u.id, e.target.value)}
                  style={{ marginLeft: "1rem" }}
                >
                  <option value="Marine">Marine</option>
                  <option value="Sergeant">Sergeant</option>
                  <option value="Lieutenant">Lieutenant</option>
                  <option value="Captain">Captain</option>
                </select>
                <select
                  value={u.squadId || ""}
                  onChange={(e) => handleSquadChange(u.id, e.target.value)}
                  style={{ marginLeft: "1rem" }}
                >
                  <option value="">Unassigned</option>
                  {squads.map(s => (
                    <option key={s.id} value={s.id}>{s.name}</option>
                  ))}
                </select>
                <button style={{ marginLeft: "0.5rem" }} onClick={() => startEdit(u)}>
                  Edit Profile
                </button>
                <button style={{ marginLeft: "0.5rem" }} onClick={() => handleDelete(u.id)}>
                  Delete
                </button>
              </>
            )}
            {editingId === u.id && (
              <div style={{
                marginTop: "0.5rem",
                border: "1px solid #ccc",
                padding: "0.5rem"
              }}>
                {editingUser.portraitImage && (
                  <div style={{ marginBottom: "0.5rem" }}>
                    <img
                      src={`https://localhost:7170/images/${editingUser.portraitImage}`}
                      alt="Portrait"
                      style={{
                        width: "100px",
                        height: "100px",
                        objectFit: "cover"
                      }}
                    />
                  </div>
                )}
                <input
                  placeholder="Display Name"
                  value={editingUser.displayName}
                  onChange={(e) => setEditingUser({ ...editingUser, displayName: e.target.value })}
                />
                <input
                  placeholder="First Name"
                  value={editingUser.firstName}
                  onChange={(e) => setEditingUser({ ...editingUser, firstName: e.target.value })}
                />
                <input
                  placeholder="Last Name"
                  value={editingUser.lastName}
                  onChange={(e) => setEditingUser({ ...editingUser, lastName: e.target.value })}
                />
                <input
                  type="number"
                  placeholder="Age"
                  value={editingUser.age}
                  onChange={(e) => setEditingUser({ ...editingUser, age: e.target.value })}
                />
                <input
                  type="number"
                  placeholder="Experience"
                  value={editingUser.experience}
                  onChange={(e) => setEditingUser({ ...editingUser, experience: e.target.value })}
                />
                <div>
                  <label>
                    Upload Portrait:
                    <input
                      type="file"
                      onChange={(e) => setSelectedFile(e.target.files[0])}
                    />
                  </label>
                </div>
                <button onClick={() => saveProfile(u.id)}>Save</button>
                <button onClick={() => setEditingId(null)}>Cancel</button>
              </div>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
}

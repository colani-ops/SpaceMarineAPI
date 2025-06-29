import { useEffect, useState } from "react";
import axios from "axios";

export default function ManageUsers() {
  const [users, setUsers] = useState([]);
  const [newUser, setNewUser] = useState({
    username: "",
    password: "",
    role: "User"
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchUsers();
  }, []);

  function fetchUsers() {
    setLoading(true);
    axios
      .get("https://localhost:7170/api/User")
      .then((res) => setUsers(res.data))
      .catch((err) => console.error("Error fetching users:", err))
      .finally(() => setLoading(false));
  }

  function handleCreate() {
    if (!newUser.username || !newUser.password) {
      alert("Username and password are required.");
      return;
    }
    axios
      .post("https://localhost:7170/api/User/register", newUser)
      .then(() => {
        setNewUser({ username: "", password: "", role: "User" });
        fetchUsers();
      })
      .catch((err) => console.error("Error creating user:", err));
  }

  function handleDelete(id) {
    if (!window.confirm("Delete this user?")) return;
    axios
      .delete(`https://localhost:7170/api/User/${id}`)
      .then(fetchUsers)
      .catch((err) => console.error("Error deleting user:", err));
  }

  function handleRoleChange(id, role) {
    axios
      .put(`https://localhost:7170/api/User/${id}/role`, { role })
      .then(fetchUsers)
      .catch((err) => console.error("Error updating role:", err));
  }

  return (
    <div style={{ padding: "1rem" }}>
      <h2>Manage Users</h2>

      <h3>Create New User</h3>
      <div style={{ marginBottom: "1rem" }}>
        <input
          placeholder="Username"
          value={newUser.username}
          onChange={(e) => setNewUser({ ...newUser, username: e.target.value })}
        />
        <input
          placeholder="Password"
          type="password"
          value={newUser.password}
          onChange={(e) => setNewUser({ ...newUser, password: e.target.value })}
        />
        <select
          value={newUser.role}
          onChange={(e) => setNewUser({ ...newUser, role: e.target.value })}
        >
          <option value="User">User</option>
          <option value="Sergeant">Sergeant</option>
          <option value="Lieutenant">Lieutenant</option>
          <option value="Captain">Captain</option>
          <option value="SuperAdmin">SuperAdmin</option>
        </select>
        <button onClick={handleCreate}>Create User</button>
      </div>

      {loading ? (
        <p>Loading users...</p>
      ) : (
        <ul>
          {users.map((u) => (
            <li key={u.id} style={{ marginBottom: "0.5rem" }}>
              <strong>{u.username}</strong> ({u.role})
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
                    <option value="User">User</option>
                    <option value="Sergeant">Sergeant</option>
                    <option value="Lieutenant">Lieutenant</option>
                    <option value="Captain">Captain</option>
                  </select>
                <button
                  style={{ marginLeft: "0.5rem" }}
                  onClick={() => handleDelete(u.id)}
                >
                Delete
                </button>
                </>
                )}
          </li>
        ))}

      </ul>
      )}
    </div>
  );
}

import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";
import { useAuth } from "../AuthContext";

export default function UserProfile() {
  const { id } = useParams();
  const { user: loggedInUser } = useAuth();
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (id) {
      // Load a specific user by id
      axios
        .get(`https://localhost:7170/api/User/${id}`)
        .then((res) => setUser(res.data))
        .catch((err) => console.error("Error fetching user", err))
        .finally(() => setLoading(false));
    } else {
      // Fallback: show the logged-in user (My Profile)
      setUser(loggedInUser);
      setLoading(false);
    }
  }, [id, loggedInUser]);

  if (loading) return <p>Loading profile...</p>;
  if (!user) return <p>User not found.</p>;

  return (
    <div style={{ padding: "1rem" }}>
      <h2>{id ? "User Profile" : "My Profile"}</h2>
      {user.portraitImage && (
        <img
          src={`https://localhost:7170/images/${user.portraitImage}`}
          alt="Portrait"
          style={{ width: "100px", height: "100px", objectFit: "cover", borderRadius: "4px" }}
        />
      )}
      <p><strong>Username:</strong> {user.username}</p>
      <p><strong>Role:</strong> {user.role}</p>
      <p><strong>Squad:</strong> {user.squadId ? `Squad #${user.squadId}` : "Unassigned"}</p>
      <p><strong>Display Name:</strong> {user.displayName || "(none)"}</p>
      <p><strong>First Name:</strong> {user.firstName || "(none)"}</p>
      <p><strong>Last Name:</strong> {user.lastName || "(none)"}</p>
      <p><strong>Age:</strong> {user.age || "(none)"}</p>
      <p><strong>Experience:</strong> {user.experience || "(none)"}</p>
    </div>
  );
}

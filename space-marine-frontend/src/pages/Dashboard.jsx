import { useAuth } from "../AuthContext";
import { Link } from "react-router-dom";

export default function Dashboard() {
  const { user } = useAuth();

  return (
    <div style={{ padding: "1rem" }}>
      <h2>Welcome, {user.username}!</h2>
      <p>Role: {user.role}</p>

      {user.role === "SuperAdmin" && (
        <div>
          <h3>SuperAdmin Actions</h3>
          <ul>
            <li>
              <Link to="/manage-squads">Manage Squads</Link>
            </li>
            <li>
              <Link to="/manage-users">Manage Users</Link>
            </li>
          </ul>
        </div>
      )}

      {user.role === "User" && (
        <div>
          <p>This is your marine profile page.</p>
          {/* In the future, show profile info here */}
        </div>
      )}
    </div>
  );
}

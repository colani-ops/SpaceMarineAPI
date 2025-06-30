import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Squads from "./pages/Squads";
import SquadProfile from "./pages/SquadProfile";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Dashboard from "./pages/Dashboard";
import ProtectedRoute from "./components/ProtectedRoute";
import Manage from "./pages/Manage";
import ManageSquads from "./pages/ManageSquads";
import { useAuth } from "./AuthContext";
import ManageUsers from "./pages/ManageUsers";
import UserProfile from "./pages/UserProfile";


export default function App() {
  const { user, logout } = useAuth();

  return (
    <Router>
      <header style={{ display: "flex", justifyContent: "space-between", padding: "1rem" }}>
        <nav>
          <Link to="/">Home</Link> | <Link to="/squads">Squads</Link>
        </nav>
        <div>
          {user ? (
            <>
              <span>Welcome, {user.username}</span> |{" "}
              <Link to="/dashboard">Dashboard</Link> |{" "}
              <button onClick={logout}>Logout</button>
            </>
          ) : (
            <>
              <Link to="/login">Login</Link> | <Link to="/register">Register</Link>
            </>
          )}
        </div>
      </header>

      <Routes>
        <Route path="/" element={<h1>Welcome to the Space Marine DB</h1>} />
        <Route path="/squads" element={<Squads />} />
        <Route path="/squad/:id" element={<SquadProfile />} />
        <Route path="/user/:id" element={<UserProfile />} />
        <Route path="/register" element={<Register />} />
        <Route path="/login" element={<Login />} />

        <Route
          path="/dashboard"
          element={
            <ProtectedRoute>
              <Dashboard />
            </ProtectedRoute>
          }

        />
        <Route
          path="/manage"
          element={
            <ProtectedRoute allowedRoles={["SuperAdmin"]}>
              <Manage />
            </ProtectedRoute>
          }

        />
        <Route
          path="/manage-squads"
          element={
            <ProtectedRoute allowedRoles={["SuperAdmin"]}>
              <ManageSquads />
            </ProtectedRoute>
          }

        />

        <Route
          path="/manage-users"
          element={
            <ProtectedRoute allowedRoles={["SuperAdmin"]}>
              <ManageUsers />
            </ProtectedRoute>
          }
        />

        <Route
          path="/user-profile"
          element={
        <ProtectedRoute>
          <UserProfile />
        </ProtectedRoute>
        }
        />
      </Routes>
    </Router>
  );
}
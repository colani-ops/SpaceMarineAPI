// src/App.jsx
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import Squads from './pages/Squads';
import Marines from './pages/Marines';
import SquadProfile from './pages/SquadProfile';
import MarineProfile from './pages/MarineProfile';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import ProtectedRoute from "./components/ProtectedRoute";
import Manage from "./pages/Manage";
import ManageSquads from "./pages/ManageSquads";



export default function App() {
  return (
    <Router>
      <header style={{ display: "flex", justifyContent: "space-between", padding: "1rem" }}>
        <nav>
          <Link to="/">Home</Link> | <Link to="/squads">Squads</Link>
        </nav>
        <div>
          <Link to="/login">Login</Link> | <Link to="/register">Register</Link>
        </div>
      </header>

      <Routes>
        <Route path="/" element={<h1>Welcome to the Space Marine DB</h1>} />
        <Route path="/squads" element={<Squads />} />
        <Route path="/squad/:id" element={<SquadProfile />} />
        <Route path="/marines" element={<Marines />} />
        <Route path="/marine/:id" element={<MarineProfile />} />
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
          path="/manage/squads"
          element={
        <ProtectedRoute allowedRoles={["SuperAdmin"]}>
          <ManageSquads />
        </ProtectedRoute>
        }
        />
      </Routes>
    </Router>
  );
}

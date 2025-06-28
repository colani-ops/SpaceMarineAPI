import { Link } from "react-router-dom";

export default function Manage() {
  return (
    <div style={{ padding: "2rem" }}>
      <h2>SuperAdmin Management</h2>
      <p>Manage your squads and marines here.</p>

      <ul>
        <li><Link to="/manage/squads">Manage Squads</Link></li>
        <li><Link to="/manage/marines">Manage Marines</Link></li>
      </ul>
    </div>
  );
}
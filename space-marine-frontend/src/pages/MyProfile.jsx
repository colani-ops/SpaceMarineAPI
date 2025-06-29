import { useContext, useState } from "react";
import { AuthContext } from "../AuthContext";
import axios from "axios";

export default function MyProfile() {
  const { user } = useContext(AuthContext);
  const [newPassword, setNewPassword] = useState("");
  const [newUsername, setNewUsername] = useState("");
  const [selectedFile, setSelectedFile] = useState(null);

  if (!user) return <p>Loading...</p>;

  const handlePasswordChange = async () => {
    if (!newPassword) return alert("Enter a password.");
    await axios.put(`https://localhost:7170/api/User/${user.id}/password`, { newPassword });
    alert("Password updated.");
    setNewPassword("");
  };

  const handleUsernameChange = async () => {
    if (!newUsername) return alert("Enter a username.");
    await axios.put(`https://localhost:7170/api/User/${user.id}/username`, { newUsername });
    alert("Username updated.");
    setNewUsername("");
  };

  const handlePortraitUpload = async () => {
    if (!selectedFile) return alert("Choose a file.");
    const formData = new FormData();
    formData.append("file", selectedFile);
    const uploadRes = await axios.post("https://localhost:7170/api/upload", formData);
    await axios.put(`https://localhost:7170/api/User/${user.id}/portrait`, {
      portraitImage: uploadRes.data.filename
    });
    alert("Portrait updated.");
  };

  return (
    <div style={{ padding: "1rem" }}>
      <h2>My Profile</h2>
      {user.portraitImage && (
        <img
          src={`/images/${user.portraitImage}`}
          alt="Portrait"
          style={{ width: "150px", height: "150px", objectFit: "cover", border: "1px solid #ccc" }}
        />
      )}
      <p><strong>Username:</strong> {user.username}</p>
      <p><strong>Role:</strong> {user.role}</p>

      <div style={{ marginTop: "1rem" }}>
        <h4>Change Username</h4>
        <input
          value={newUsername}
          onChange={(e) => setNewUsername(e.target.value)}
          placeholder="New username"
        />
        <button onClick={handleUsernameChange}>Update Username</button>
      </div>

      <div style={{ marginTop: "1rem" }}>
        <h4>Change Password</h4>
        <input
          value={newPassword}
          onChange={(e) => setNewPassword(e.target.value)}
          placeholder="New password"
          type="password"
        />
        <button onClick={handlePasswordChange}>Update Password</button>
      </div>

      <div style={{ marginTop: "1rem" }}>
        <h4>Change Portrait</h4>
        <input type="file" onChange={(e) => setSelectedFile(e.target.files[0])} />
        <button onClick={handlePortraitUpload}>Upload Portrait</button>
      </div>
    </div>
  );
}

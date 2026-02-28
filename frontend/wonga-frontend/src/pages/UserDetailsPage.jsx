import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getMe } from '../services/api';
import './UserDetailsPage.css';

export default function UserDetailsPage() {
  const [user, setUser] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    getMe()
      .then(res => setUser(res.data))
      .catch(() => {
        localStorage.removeItem('token');
        navigate('/');
      });
  }, [navigate]);

  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/');
  };

  if (!user) return <div className="loading">Loading...</div>;

  return (
    <div className="dashboard-wrapper">
      <div className="dashboard-card">
        <div className="card-content">
          <div className="dash-brand">
            <div className="dash-brand-dot" />
            <span className="dash-brand-name">Wonga</span>
          </div>
          <div className="avatar">
            {user.firstName[0]}{user.lastName[0]}
          </div>
          <h1 className="dash-name">{user.firstName} {user.lastName}</h1>
          <p className="dash-email">{user.email}</p>
          <div className="info-grid">
            <div className="info-item">
              <span className="label">First Name</span>
              <span className="value">{user.firstName}</span>
            </div>
            <div className="info-item">
              <span className="label">Last Name</span>
              <span className="value">{user.lastName}</span>
            </div>
            <div className="info-item full">
              <span className="label">Email Address</span>
              <span className="value">{user.email}</span>
            </div>
          </div>
          <button className="btn-logout" onClick={handleLogout}>Sign Out</button>
        </div>
      </div>
    </div>
  );
}
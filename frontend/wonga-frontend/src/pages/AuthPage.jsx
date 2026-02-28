import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { register, login } from '../services/api';
import './AuthPage.css';

const PARTICLE_COUNT = 25;

export default function AuthPage() {
  const [isLogin, setIsLogin] = useState(true);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const [particles, setParticles] = useState([]);
  const navigate = useNavigate();

  const [loginData, setLoginData] = useState({ email: '', password: '' });
  const [registerData, setRegisterData] = useState({
    firstName: '', lastName: '', email: '', password: ''
  });

  useEffect(() => {
    const p = Array.from({ length: PARTICLE_COUNT }, (_, i) => ({
      id: i,
      left: `${Math.random() * 100}%`,
      delay: `${Math.random() * 10}s`,
      duration: `${6 + Math.random() * 8}s`,
      size: `${1 + Math.random() * 3}px`,
      opacity: Math.random() * 0.6 + 0.2,
    }));
    setParticles(p);
  }, []);

  const handleLogin = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      const res = await login(loginData);
      localStorage.setItem('token', res.data.token);
      navigate('/dashboard');
    } catch {
      setError('Invalid email or password.');
    } finally {
      setLoading(false);
    }
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      await register(registerData);
      setIsLogin(true);
      setError('');
    } catch {
      setError('Registration failed. Email may already be in use.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-wrapper">
      <div className="particles">
        {particles.map(p => (
          <div key={p.id} className="particle" style={{
            left: p.left,
            width: p.size,
            height: p.size,
            animationDelay: p.delay,
            animationDuration: p.duration,
          }} />
        ))}
      </div>

      <div className={`auth-container ${!isLogin ? 'slide-to-register' : ''}`}>
        <div className="grid-lines" />

        {/* Login Form */}
        <div className="form-section login-section">
          <div className="brand-mark">
            <div className="brand-dot" />
            <span className="brand-name">Wonga</span>
          </div>
          <h2>Welcome back</h2>
          <p className="subtitle">Sign in to your account</p>
          {error && isLogin && <div className="error">{error}</div>}
          <form onSubmit={handleLogin}>
            <div className="input-group">
              <input
                type="email"
                placeholder="Email address"
                value={loginData.email}
                onChange={e => setLoginData({...loginData, email: e.target.value})}
                required
              />
              <span className="icon">✉</span>
            </div>
            <div className="input-group">
              <input
                type="password"
                placeholder="Password"
                value={loginData.password}
                onChange={e => setLoginData({...loginData, password: e.target.value})}
                required
              />
              <span className="icon">🔒</span>
            </div>
            <button type="submit" className="btn-primary" disabled={loading}>
              {loading ? 'Signing in...' : 'Sign In →'}
            </button>
          </form>
        </div>

        {/* Register Form */}
        <div className="form-section register-section">
          <div className="brand-mark">
            <div className="brand-dot" />
            <span className="brand-name">Wonga</span>
          </div>
          <h2>Create account</h2>
          <p className="subtitle">Join us today</p>
          {error && !isLogin && <div className="error">{error}</div>}
          <form onSubmit={handleRegister}>
            <div className="input-row">
              <div className="input-group">
                <input
                  type="text"
                  placeholder="First name"
                  value={registerData.firstName}
                  onChange={e => setRegisterData({...registerData, firstName: e.target.value})}
                  required
                />
              </div>
              <div className="input-group">
                <input
                  type="text"
                  placeholder="Last name"
                  value={registerData.lastName}
                  onChange={e => setRegisterData({...registerData, lastName: e.target.value})}
                  required
                />
              </div>
            </div>
            <div className="input-group">
              <input
                type="email"
                placeholder="Email address"
                value={registerData.email}
                onChange={e => setRegisterData({...registerData, email: e.target.value})}
                required
              />
              <span className="icon">✉</span>
            </div>
            <div className="input-group">
              <input
                type="password"
                placeholder="Password"
                value={registerData.password}
                onChange={e => setRegisterData({...registerData, password: e.target.value})}
                required
              />
              <span className="icon">🔒</span>
            </div>
            <button type="submit" className="btn-primary" disabled={loading}>
              {loading ? 'Creating account...' : 'Create Account →'}
            </button>
          </form>
        </div>

        {/* Sliding Panel */}
        <div className="slider-panel">
          <div className={`slider ${!isLogin ? 'slide-right' : ''}`}>
            <div className="panel-content">
              <p className="panel-logo">WONGA</p>
              {isLogin ? (
                <>
                  <h2>New here?</h2>
                  <p>Create an account and start your journey with us today.</p>
                  <button className="btn-outline" onClick={() => { setIsLogin(false); setError(''); }}>
                    Get Started
                  </button>
                </>
              ) : (
                <>
                  <h2>Welcome back!</h2>
                  <p>Already have an account? Sign in to continue.</p>
                  <button className="btn-outline" onClick={() => { setIsLogin(true); setError(''); }}>
                    Sign In
                  </button>
                </>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
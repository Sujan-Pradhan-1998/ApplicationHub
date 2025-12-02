import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Form, Input, Button, Divider, Modal, Progress } from "antd";
import { login } from "../../services/authService";
import { setBearerToken } from "../../utils/auth";
import { showErrorMessage, showSuccessMessage } from "../../services/messageService";
import "../../styles/Login.scss";
import { post } from "../../services/ajaxService";
import type { LoginModel, RegisterModel, UserModel } from "../../models/LoginModel";


const LoginPage: React.FC = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [registerLoading, setRegisterLoading] = useState(false);
  const [showRegisterModal, setShowRegisterModal] = useState(false);
  const [form] = Form.useForm();

  const onFinish = async (values: LoginModel) => {
    try {
      setLoading(true);
      const res = await login(values);
      if (res) {
        setBearerToken(res || "");
        showSuccessMessage("Login successful!");
        navigate("/application-form");
      } else {
        showErrorMessage("Login failed");
      }
    } catch (err: any) {
      showErrorMessage(err.response?.data?.message || "Login failed");
    } finally {
      setLoading(false);
    }
  };

  const checkPasswordStrength = (password: string) => {
    // Minimum 8 characters, at least 1 uppercase, 1 lowercase, 1 number, 1 special character
    const strongPasswordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
    return strongPasswordRegex.test(password);
  };

  const onRegisterFinish = async (values: RegisterModel) => {
    if (values.email !== values.confirmEmail) {
      showErrorMessage("Emails do not match");
      return;
    }

    if (values.password !== values.confirmPassword) {
      showErrorMessage("Passwords do not match");
      return;
    }

    if (!checkPasswordStrength(values.password)) {
      showErrorMessage(
        "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character."
      );
      return;
    }

    try {
      setRegisterLoading(true);
      const payload = {
        firstName: values.firstName,
        middleName: values.middleName,
        lastName: values.lastName,
        currentCompany: values.currentCompany,
        email: values.email,
        password: values.password,
      };

      const res = await post<UserModel>('user/registeruser', payload);
      if (res) {
        showSuccessMessage("Registration successful!");
        setShowRegisterModal(false);
        form.resetFields();
        navigate("/login");
      } else {
        showErrorMessage("Registration failed");
      }
    } catch (err: any) {
      showErrorMessage(err.response?.data?.message || "Registration failed");
    } finally {
      setRegisterLoading(false);
    }
  };


  const handleRegisterCancel = () => {
    setShowRegisterModal(false);
    form.resetFields()
  }

  const [password, setPassword] = useState("");

  const getPasswordStrength = (pwd: string) => {
    if (!pwd) return { level: 0, text: "" };

    let score = 0;
    if (pwd.length >= 8) score++;
    if (/[A-Z]/.test(pwd)) score++;
    if (/[a-z]/.test(pwd)) score++;
    if (/\d/.test(pwd)) score++;
    if (/[@$!%*?&]/.test(pwd)) score++;

    let text = "";
    switch (score) {
      case 0:
      case 1:
      case 2:
        text = "Weak";
        break;
      case 3:
      case 4:
        text = "Medium";
        break;
      case 5:
        text = "Strong";
        break;
    }

    return { level: score * 20, text };
  };

  const strength = getPasswordStrength(password);

  return (
    <div className="login-container">
      <div className="login-box">
        <h2 className="login-title">Application Hub Login</h2>

        <Form name="login" onFinish={onFinish} layout="vertical">
          <Form.Item
            label="Email"
            name="email"
            rules={[{ required: true, message: "Please input your email!" }]}
          >
            <Input />
          </Form.Item>

          <Form.Item
            label="Password"
            name="password"
            rules={[{ required: true, message: "Please input your password!" }]}
          >
            <Input.Password />
          </Form.Item>

          <Form.Item>
            <Button type="primary" htmlType="submit" loading={loading} block>
              Login
            </Button>
          </Form.Item>

          <Divider>Or</Divider>

          <Form.Item>
            <Button type="default" onClick={() => setShowRegisterModal(true)} block>
              Register New User
            </Button>
          </Form.Item>
        </Form>
      </div>

      <Modal open={showRegisterModal} onCancel={handleRegisterCancel} closable footer={null} style={{ top: 50 }}>
        <Form form={form} name="register" onFinish={onRegisterFinish} layout="vertical">
          <Form.Item style={{ marginBottom: 15 }}
            label="First Name"
            name="firstName"
            rules={[{ required: true, message: "Please input your first name!" }]}
          >
            <Input />
          </Form.Item>

          <Form.Item style={{ marginBottom: 15 }} label="Middle Name" name="middleName">
            <Input />
          </Form.Item>

          <Form.Item style={{ marginBottom: 15 }}
            label="Last Name"
            name="lastName"
            rules={[{ required: true, message: "Please input your last name!" }]}
          >
            <Input />
          </Form.Item>

          <Form.Item style={{ marginBottom: 15 }}
            label="Current Company"
            name="currentCompany"
            rules={[{ required: true, message: "Please input your current company!" }]}
          >
            <Input />
          </Form.Item>

          <Form.Item style={{ marginBottom: 15 }}
            label="Email"
            name="email"
            rules={[
              { required: true, message: "Please input your email!" },
              { type: "email", message: "Please enter a valid email!" },
            ]}
          >
            <Input />
          </Form.Item>

          <Form.Item style={{ marginBottom: 15 }}
            label="Confirm Email"
            name="confirmEmail"
            dependencies={["email"]}
            rules={[
              { required: true, message: "Please confirm your email!" },
              ({ getFieldValue }) => ({
                validator(_, value) {
                  if (!value || getFieldValue("email") === value) {
                    return Promise.resolve();
                  }
                  return Promise.reject(new Error("Emails do not match!"));
                },
              }),
            ]}
          >
            <Input onPaste={(e) => e.preventDefault()} />
          </Form.Item>

          <Form.Item
            style={{ marginBottom: 5 }}
            label="Password"
            name="password"
            rules={[{ required: true, message: "Please input your password!" }]}
          >
            <Input.Password
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Enter your password"
            />
          </Form.Item>

          {/* Password strength indicator outside Form.Item */}
          {password && (
            <div style={{ marginBottom: 15 }}>
              <Progress
                percent={strength.level}
                size="small"
                status={
                  strength.text === "Weak"
                    ? "exception"
                    : strength.text === "Medium"
                      ? "normal"
                      : "success"
                }
                showInfo={false}
              />
              <span style={{ fontSize: 12, color: "#555" }}>{strength.text}</span>
            </div>
          )}

          <Form.Item style={{ marginBottom: 15 }}
            label="Confirm Password"
            name="confirmPassword"
            dependencies={["password"]}
            rules={[
              { required: true, message: "Please confirm your password!" },
              ({ getFieldValue }) => ({
                validator(_, value) {
                  if (!value || getFieldValue("password") === value) {
                    return Promise.resolve();
                  }
                  return Promise.reject(new Error("Passwords do not match!"));
                },
              }),
            ]}
          >
            <Input.Password onPaste={(e) => e.preventDefault()} />
          </Form.Item>

          <Form.Item>
            <Button type="primary" htmlType="submit" loading={registerLoading} block>
              Register
            </Button>
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default LoginPage;

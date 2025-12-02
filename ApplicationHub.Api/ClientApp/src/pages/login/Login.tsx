import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Form, Input, Button, Divider, Modal } from "antd";
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

  const onRegisterFinish = async (values: RegisterModel) => {
    if (values.email !== values.confirmEmail) {
      showErrorMessage("Emails do not match");
      return;
    }

    if (values.password !== values.confirmPassword) {
      showErrorMessage("Passwords do not match");
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
  }

  const handleRegisterCancel = () => {
    setShowRegisterModal(false);
    form.resetFields()
  }

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

          <Form.Item style={{ marginBottom: 15 }}
            label="Password"
            name="password"
            rules={[{ required: true, message: "Please input your password!" }]}
          >
            <Input.Password />
          </Form.Item>

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

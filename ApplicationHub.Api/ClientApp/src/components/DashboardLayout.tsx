import React, { useEffect, useState } from 'react';
import {
  FileAddOutlined,
  LoginOutlined,
  MenuFoldOutlined,
  MenuUnfoldOutlined
} from '@ant-design/icons';
import type { MenuProps } from 'antd';
import { Button, Divider, Layout, Menu, theme, Typography } from 'antd';
import { logout } from '../utils/auth';
import { useNavigate, Outlet } from 'react-router-dom';
import "../styles/Dashboard.scss";
import logo from '../assets/logo.png';
import { get, post } from '../services/ajaxService';
import type { UserMeta } from '../models/UserMetaModel';

const { Header, Content, Footer, Sider } = Layout;

const siderStyle: React.CSSProperties = {
  overflow: 'auto',
  height: '100vh',
  position: 'sticky',
  insetInlineStart: 0,
  top: 0,
  bottom: 0,
  scrollbarWidth: 'thin',
  scrollbarGutter: 'stable',
};

const DashboardLayout = () => {
  const navigate = useNavigate();
  const [collapsed, setCollapsed] = useState(localStorage.getItem("collapsed") == "true" || false);

  const handleLogout = async () => {
    await post<any>('login/logout', {});
    logout();
    navigate("/login");
  };

  const {
    token: { colorBgContainer, borderRadiusLG },
  } = theme.useToken();

  const items: MenuProps['items'] = [
    {
      key: '1',
      icon: <FileAddOutlined />,
      label: 'Application',
      onClick: () => navigate('/application-form'),
    },
  ];

  const [userMeta, setUserMeta] = useState<UserMeta>({
    currentCompany: '',
    email: '',
    id: '',
    userName: ''
  });

  const getUserMeta = async () => {
    const data = await get<UserMeta>('user/usermeta')
    if (data) {
      setUserMeta(data.data)
    }
  }

  useEffect(() => {
    getUserMeta()
  }, [])


  return (
    <Layout hasSider>
      <Sider
        style={siderStyle}
        collapsible
        collapsed={collapsed}
        onCollapse={(value) => {
          setCollapsed(value)
          localStorage.setItem("collapsed", `${value}`)
        }}
      >
        <div className="sidebar-logo">
          <a onClick={() => navigate("/dashboard")}>
            <img src={logo} alt="Logo" />
          </a>
        </div>
        {!collapsed ? <Divider size='small' /> : null}
        <Menu theme="dark" mode="inline" defaultSelectedKeys={['1']} items={items} />
      </Sider>

      <Layout>
        <Header
          style={{
            padding: 0,
            background: colorBgContainer,
            display: 'flex',
            alignItems: 'center',
          }}
        >
          <Button
            type="text"
            icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
            onClick={() => {
              setCollapsed(!collapsed)
              localStorage.setItem("collapsed", `${!collapsed}`)
            }}
            style={{ fontSize: 16, width: 64, height: 64 }}
          />
          <Typography.Text strong>User : </Typography.Text>
          <Typography.Text>{userMeta.userName}</Typography.Text>
          <Button
            type="text"
            icon={<LoginOutlined />}
            onClick={handleLogout}
            style={{
              fontSize: 16,
              height: 64,
              marginLeft: 'auto',
            }}
          >
            Logout
          </Button>
        </Header>

        <Content style={{ margin: '24px 16px 0' }}>
          <div
            style={{
              padding: 24,
              background: colorBgContainer,
              borderRadius: borderRadiusLG,
              minHeight: '80vh',
            }}
          >
            {/* Protected pages render here */}
            <Outlet />
          </div>
        </Content>

        <Footer style={{ textAlign: 'center' }}>
          Application Hub ©{new Date().getFullYear()}
        </Footer>
      </Layout>
    </Layout>
  );
};

export default DashboardLayout;

import { render } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import React from 'react';
import { Loader } from '../../components/Loader';

describe('Loader', () => {
  it('renders children inside spinner', () => {
    const child = React.createElement('div', null, 'Child Content');
    const element = React.createElement(Loader, { tip: 'Loading' }, child);
    const { getByText } = render(element);
    expect(getByText('Child Content')).toBeTruthy();
    expect(getByText('Loading')).toBeTruthy();
  });

  it('renders brand type spinner', () => {
    const element = React.createElement(Loader, { type: 'brand' });
    const { container } = render(element);
    expect(container.querySelector('.ant-spin')).toBeTruthy();
  });

  it('renders circle type spinner with LoadingOutlined icon', () => {
    const element = React.createElement(Loader, { type: 'circle' });
    const { container } = render(element);
    expect(container.querySelector('.anticon-loading')).toBeTruthy();
  });
});

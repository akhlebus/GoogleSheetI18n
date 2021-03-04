import React, { useState, useCallback } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { LanguageDetector } from './LanguageDetector';
import { useTranslation } from 'react-i18next';

export function NavMenu () {
  const { t } = useTranslation();
  const [isCollapsed, setIsCollapsed] = useState(true);
  const toggleNavbar = useCallback(() => setIsCollapsed(value => !value), [setIsCollapsed]);

  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
        <Container>
          <NavbarBrand tag={Link} to="/">GoogleSheetI18n.Api.SimpleWebApi</NavbarBrand>
          <NavbarToggler onClick={toggleNavbar} className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!isCollapsed} navbar>
            <LanguageDetector />
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/">{t('global:navbar.home')}</NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/introduction">{t('global:navbar.get-started')}</NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/administration">{t('global:navbar.administration')}</NavLink>
              </NavItem>
            </ul>
          </Collapse>
        </Container>
      </Navbar>
    </header>
  );
}

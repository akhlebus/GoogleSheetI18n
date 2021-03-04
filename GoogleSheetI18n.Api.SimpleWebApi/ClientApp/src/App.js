import React, { Suspense, useState, useEffect } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Introduction } from './components/Introduction';
import { Administration } from './components/Administration';
import { Loader } from './components/Loader';
import { ErrorSummary } from './components/ErrorSummary';
import { I18nSettingsContext } from './I18nSettingsContext';

import './custom.css';

export default function App() {
  const [error, setError] = useState(null);
  const [isLoaded, setIsLoaded] = useState(false);
  const [i18nSettings, setI18nSettings] = useState({});

  useEffect(() => {
    window.fetch('/i18n/settings')
      .then(res => res.json())
      .then(
        (result) => {
          setIsLoaded(true);
          setI18nSettings(result);
        },
        (error) => {
          setIsLoaded(true);
          setError(error);
        }
      );
  }, []);

  return (
    <Suspense fallback="Loading...">
      {error && <ErrorSummary message={error.message} />}
      {!error && !isLoaded && <Loader />}
      {!error && isLoaded && <I18nSettingsContext.Provider value={i18nSettings}>
        <Layout>
          <Route exact path='/' component={Home} />
          <Route path='/introduction' component={Introduction} />
          <Route path='/administration' component={Administration} />
        </Layout>
      </I18nSettingsContext.Provider>}
    </Suspense>
  );
}

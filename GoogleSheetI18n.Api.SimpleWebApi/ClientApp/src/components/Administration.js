import React, { useContext, useCallback, useState } from 'react';
import { Loader } from './Loader';
import { ErrorSummary } from './ErrorSummary';
import { useTranslation } from 'react-i18next';
import { I18nSettingsContext } from '../I18nSettingsContext';

export function Administration() {
  const { t } = useTranslation();
  const [error, setError] = useState(null);
  const [isLoaded, setIsLoaded] = useState(true);
  const i18nSettings = useContext(I18nSettingsContext);
  const linkToGoogleSheet = `https://docs.google.com/spreadsheets/d/${i18nSettings.spreadsheetId}`;

  const clearCache = useCallback(() => {
    window.fetch('/i18n/empty-cache', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'X-Goog-Channel-ID': i18nSettings.currentChannel.channelId,
        'X-Goog-Resource-ID': i18nSettings.currentChannel.resourceId
      },
      body: {}
    })
      .then(
        (result) => {
          window.location.reload();
          setIsLoaded(true);
        },
        (error) => {
          setIsLoaded(true);
          setError(error);
        }
    );
    setIsLoaded(false);
  }, [i18nSettings, setIsLoaded, setError]);

  if (error) {
    return <ErrorSummary message={error.message} />;
  }

  return (
    <div>
      <h1>{t('admin-page:administration')}</h1>
      <h3>{t('admin-page:basic-actions')}</h3>
      {isLoaded && <div>
        <a href={linkToGoogleSheet} target="_blank" rel="noreferrer" className="btn btn-lg btn-success mb-3 mr-2">{t('admin-page:view-google-sheet')}</a>
        <button onClick={clearCache} className="btn btn-lg btn-outline-secondary mb-3">{t('admin-page:clear-cache')}</button>
      </div>}
      {!isLoaded && <Loader />}
    </div>
  );
}
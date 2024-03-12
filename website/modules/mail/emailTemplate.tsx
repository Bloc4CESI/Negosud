import * as React from 'react';

interface EmailTemplateProps {
    sujet: string;
    email: string;
    message: string;
}

export const EmailTemplate: React.FC<Readonly<EmailTemplateProps>> = ({email, sujet,  message}) => (
    <div>
        <h1>Nouveau message de la part de {email}</h1>
        <h2>Son message est a propos de {sujet}</h2>
        <p>{message}</p>
    </div>
);
import { Resend } from 'resend';
import * as React from 'react';
import {EmailTemplate} from "../../../../modules/mail/emailTemplate";

const resend = new Resend('re_3hijQvi3_D587fxB31183oPVtsTWk1Q2h');

export async function POST(request: Request) {
    const { email, sujet, message } = await request.json();

    try {
        const { data, error } = await resend.emails.send({
            from: 'Acme <onboarding@resend.dev>',
            to: 'adrien@carjager.com',
            subject: "Demande de contact via Negosud",
            react: EmailTemplate({ message, sujet, email }) as React.ReactElement,
        });

        if (error) {
            return Response.json({ error });
        }

        return Response.json({ data });
    } catch (error) {
        return Response.json({error});
    }
}
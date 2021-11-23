import React from "react";
import { GeistProvider, CssBaseline, Page, Display } from "@geist-ui/react";

export default function App() {
    return (
        <GeistProvider>
            <CssBaseline />
            <Page>
                <Display>
                    Test?
                </Display>
            </Page>
        </GeistProvider>
    );
}
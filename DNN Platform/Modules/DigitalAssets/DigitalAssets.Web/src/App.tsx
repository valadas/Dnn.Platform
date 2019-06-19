import * as React from 'react';

export interface AppProps {
    moduleId: number;
}

const App: React.FunctionComponent<AppProps> = ({ moduleId }: AppProps): React.ReactElement => <div>
    <h1>Hello from react with typescript</h1>
    <p>This is moduleId: {moduleId}</p>
</div>

export default App;
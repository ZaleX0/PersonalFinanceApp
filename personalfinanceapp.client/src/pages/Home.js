import React, { useState } from 'react'
import { Dialog } from 'primereact/dialog';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';



export default function Home() {

  const [state, setState] = useState(false);

  return (
    <div>
      <Card>
        <Card>
          <p>uhseifu</p>
        </Card>
      <Dialog visible={state} onHide={() => setState(false)}>
      </Dialog>

      <Button label="Show" onClick={() => setState(true)} />
      </Card>
    </div>
  )
}
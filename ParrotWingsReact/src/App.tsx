import React, { useState } from 'react'
import NavigationBar from './components/NavigationBar'
import Main from './components/Main'
import { Image } from 'react-bootstrap'
import { IContextProps } from './components/auxiliary/Interfaces'

export const PWContext = React.createContext<IContextProps>({})

export default function App() {
  const [isAuthorized, setIsAuthorized] = useState(localStorage.getItem('userId') !== null)
  const [userBalance, setUserBalance] = useState('')
  const [isLoading, setIsLoading] = useState(false)

  return (
    <div className='app'>
      <PWContext.Provider
        value={{
          isAuthorized,
          setIsAuthorized,
          userBalance,
          setUserBalance,
          setIsLoading,
        }}
      >
        <NavigationBar/>
        <Main/>
      </PWContext.Provider>
      <div className={`loader ${!isLoading ? 'hidden' : ''}`}>
        <Image src='/images/loader.gif' />
      </div>
    </div>
  )
}

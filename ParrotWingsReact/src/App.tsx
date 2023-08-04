import React, { useState } from 'react'
import NavigationBar from './components/NavigationBar'
import Main from './components/Main'
import { Image } from 'react-bootstrap'

export default function App() {
  const [isAuthorized, setIsAuthorized] = useState(localStorage.getItem('userId') !== null)
  const [userBalance, setUserBalance] = useState('')
  const [isLoading, setIsLoading] = useState(false)

  return (
    <div className='app'>
      <NavigationBar
        userBalance={userBalance}
        setUserBalance={setUserBalance}
        isAuthorized={isAuthorized}
        setIsAuthorized={setIsAuthorized}
        setIsLoading={setIsLoading}
      />
      <Main
        userBalance={userBalance}
        setUserBalance={setUserBalance}
        isAuthorized={isAuthorized}
        setIsAuthorized={setIsAuthorized}
        setIsLoading={setIsLoading}
      />
      <div className={`loader ${!isLoading ? 'hidden' : ''}`}>
        <Image src='/images/loader.gif' />
      </div>
    </div>
  )
}

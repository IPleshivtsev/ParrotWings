import React from 'react'
import { Routes, Route, Navigate } from 'react-router-dom'
import Transfer from './pages/Transfer/Transfer'
import Replenishment from './pages/Replenishment/Replenishment'
import Help from './pages/Help/Help'
import LoginUser from './pages/LoginUser/LoginUser'
import RegisterUser from './pages/RegisterUser/RegisterUser'

export default function Main() {
  return (
    <div className={'container body-content'}>
      <Routes>
        <Route path='/' element={<Navigate to='/transfer' replace />} />
        <Route path='/transfer' element={<Transfer/>} />
        <Route path='/replenishment' element={<Replenishment/>} />
        <Route path='/loginUser' element={<LoginUser/>} />
        <Route path='/registerUser' element={<RegisterUser/>} />
        <Route path='/help' element={<Help />} />
      </Routes>
      <hr className={'main-hr'} />
      <footer>
        <p>{new Date().getFullYear()}, ParrotWings 18+</p>
      </footer>
    </div>
  )
}

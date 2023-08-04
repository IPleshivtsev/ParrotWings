import React, { useState, useRef, useEffect } from 'react'
import './NavigationBar.less'
import { NavLink } from 'react-router-dom'
import { Image } from 'react-bootstrap'
import { IUnionProps } from './auxiliary/Interfaces'

export default function NavigationBar({ userBalance, setUserBalance, isAuthorized, setIsAuthorized }: IUnionProps) {
  const menuRef = useRef(null)
  const userNameMainRef = useRef(null)
  const userNameChildRef = useRef(null)
  const [isNavExpanded, setIsNavExpanded] = useState(false)
  const [userName, setUserName] = useState('')

  const handleOnClickOutside = () => {
    setIsNavExpanded(false)
  }

  const handleLogoff = () => {
    setIsAuthorized(false)
    localStorage.clear()
  }

  useEffect(() => {
    if (isAuthorized) {
      setUserName(localStorage.getItem('userName'))
      setUserBalance(localStorage.getItem('userBalance'))
    }
  }, [isAuthorized])

  useEffect(() => {
    const handleClickOutside = (event: any) => {
      if (menuRef.current && !menuRef.current.contains(event.target)) {
        handleOnClickOutside && handleOnClickOutside()
      }
    }
    document.addEventListener('click', handleClickOutside, true)
    return () => {
      document.removeEventListener('click', handleClickOutside, true)
    }
  }, [handleOnClickOutside])

  useEffect(() => {
    if (userNameMainRef.current !== null && userNameChildRef.current !== null) {
      if (window.innerWidth > 932 && userNameChildRef.current.innerHTML.length > 7) {
        userNameMainRef.current.innerHTML = userNameChildRef.current.innerHTML.substring(0, 7) + '...'
      } else {
        userNameMainRef.current.innerHTML = userNameChildRef.current.innerHTML
      }
    }
  }, [window.onresize, userNameMainRef.current, isAuthorized])

  return (
    <div className='menu' ref={menuRef}>
      <nav className='navigation'>
        <NavLink to='/transfer'>
          <Image style={{ width: '200px' }} src='/images/parrotWings.png' />
        </NavLink>
        <button
          className='menu-button'
          onClick={() => {
            setIsNavExpanded(!isNavExpanded)
          }}
        >
          <Image className={'menu-icon'} src='/images/menu-icon.svg' />
        </button>
        <div className={`navigation-menu ${isNavExpanded ? 'expanded' : ''}`}>
          <ul>
            <li>
              <NavLink to='/transfer'>Перевод</NavLink>
            </li>
            <li>
              <NavLink to='/replenishment'>Пополнение счета</NavLink>
            </li>
            <li>
              <NavLink to='/help'>Помощь</NavLink>
            </li>

            {isAuthorized && (
              <div className={'navigation-menu-profile'}>
                <div>
                  <ul>
                    <li>
                      <span className={'user-name'} ref={userNameMainRef}>
                        {userName}
                      </span>
                      <span className={'balance'}>{userBalance} PW</span>
                    </li>
                  </ul>
                </div>
                <div className='profile-dropdown-menu'>
                  <ul style={{ display: 'block' }}>
                    <li style={{ textAlign: 'center' }}>
                      <span className={'user-name'} ref={userNameChildRef}>
                        {userName}
                      </span>
                      <span className={'balance'}>{userBalance}&nbsp;PW</span>
                    </li>
                  </ul>
                  <a>Настройки</a>
                  <a onClick={handleLogoff}>Выйти</a>
                </div>
              </div>
            )}
            {!isAuthorized && (
              <li>
                <NavLink className={'navigation-menu-profile'} to='/loginUser'>
                  Войти
                </NavLink>
              </li>
            )}
          </ul>
        </div>
      </nav>
    </div>
  )
}

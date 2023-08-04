import React from 'react'
import './BannerBlock.less'
import { Image } from 'react-bootstrap'
import { NavLink } from 'react-router-dom'
import { IBannerBlockProps } from '../../auxiliary/Interfaces'

export function BannerBlock({ isTransfer }: IBannerBlockProps) {
  return (
    <div className={'union-class'}>
      <div style={{ margin: 'auto' }}>
        <label className={'first-label'}>
          {isTransfer ? 'Переводите деньги легко и быстро с Parrot Wings' : 'Пополняйте свой счет с карт любых банков'}
        </label>
        <label className={'second-label'}>Для регистрации нужны только Ваше имя и электронная почта. Все просто!</label>
        <NavLink to='/registerUser'>Зарегистрироваться</NavLink>
      </div>
      <div className={'image-cont'}>
        <Image src={isTransfer ? '/images/bannerTransfer.png' : '/images/bannerReplenishment.png'} />
      </div>
    </div>
  )
}

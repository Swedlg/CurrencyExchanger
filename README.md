# **CurrencyExchanger**

Проект для получения истории значений курсов валют с сервиса ЦБ

---

Проект представляет собой микросервисную архитектуру с 3 решениями:

- **Crawler**. Web Api проект для выгрузки информации о валютных котировках с api центробанка.
- **Converter**. Hosted Service (Worker) проект для вычисления курсов валют относительно друг друга (т.к. в API центробанка курсы валют только относительно рубля).
- **Storage**. Web Api проект для получения выборок по валютным котировкам по датам, а также содержит репозиторий хранения различной информации о валютах.

Также присутствует еще одно решение - **ExchangeData** - Библиотека с DTO моделями для их передачи между микросервисами с использованием **RabbitMQ** и **MassTransit**. Я выложил ее на NuGet.

---

Для создания задач (в том числе и повторяемых) использовалась библиотека **Hangfire**.

---

Для передачи DTO между микросервисами использовалсь модель *Publicher/Subscriber*

Очень обобщенное представление о потоках информации на картине:

![Тут должна была быть картинка со схемой](https://i.ibb.co/f2PwN9F/fsdfdsd.png)

DTO модели:

- **CurrencyInfoListDTO** - справочная информация о валютах

- **RubleQuotesByDateDTO** - котировки валют по датам относительно рубля

- **CurrencyQuotesByDateListDTO** - котировки валют по датам относительно друг друга, а не только относительно рубля

Базы Данных:

- **CurrencyNotificationServiceDb** - База данных для хранения задач Hangfire и даз загрузок валют

- **CurrencyStorageDb** - База данных для хранения справочной информации о валютах и информации о валютных котировках по датам

Строки подключения к БД и RabbitMQ передаются через окружение. В моем случае это

**Swedlg_CurrencyExchanger_CurrencyNotificationConnectionDbStringPostgres**

Host=localhost; Port=5432; Database=CurrencyNotificationServiceDb; Username=swed19; Password=postgres

**Swedlg_CurrencyExchanger_CurrencyStorageConnectionDbString**

Host=localhost; Port=5432; Database=CurrencyStorageDb; Username=swed19; Password=postgres

**Swedlg_CurrencyExchanger_RabbitServer**

{
    "RabbitServer":
    {
        "Url": "localhost",
        "Host": "currency-exchanger",
        "User": "currency-exchanger-guest", "Password": "currency-exchanger-guest"
    }
}

P.S. При попытке подключиться к RabbitMQ контейнеру через браузер (например по http://localhost:15672/#/users) может ничего не произойти. У меня всегда работает с Firefox, но с Chrome срабатывает почему-то через раз. Возможно вам нужно будет сделать жесткую перезагрузку страницы.
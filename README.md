# Игра съедобное-несъедобное
Игроку поочередно показывают карточки с картинками.

Если на картинке съедобный предмет, игрок должен свайпнуть карточку вправо, иначе - влево.

Пример игры: http://test-task-eatable.s3-website-eu-west-1.amazonaws.com

Ассеты для игры: https://files3.1c.ru/s/yCX5XcsRf99EBRD

При верном выборе игрок получает в награду 1 звезду.

При ошибке - теряет 1 жизнь. Жизней всего 3 (настройка).

На выбора дается ограниченое время - 3с. Если игрок не успел выбрать за это время, считается что он ошибся. Т.е. так-же теряет 1 жизнь.

После потери всех жизней игра заканчивается.

Появляется окно с заработанными звездами кнопкой “начать заново”

Интерфейс - счетчик заработанных звезд, количество жизней, таймер

# Технические требования
1. Для создания карточек использовать паттерн фабрика. Созданный объект помещать в родителя, а не в рут сцены.
2. Для взаимодействия между объектами применять паттерн шина событий
3. Свайпы обрабатывать через IBeginDragHandler, IDragHandler, IEndDragHandler
4. Картинки для карточек загружать динамически применяя технологию Addressable Assets. Отработанные сразу выгружать.
5. Настройки игры вынести в конфигурационный файл(количество жизней, таймаут для выбора, макс. комбо)
6. Состояния карточек реализовать через стейт машину-аниматор.
7. Для интерфейса ипользовать Unity UI

Время выполнения 3ч
Версия unity 2019.4.30f1
# Критерии оценки
Задание нужно сдать в формате открытого репозитория на http://github.com
Оценивать будем:
1. Качество кода. 
2. Построение проекта
3. Структуру сцены
4. Качество анимаций и эффектов

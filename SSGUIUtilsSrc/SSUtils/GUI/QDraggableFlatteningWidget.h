#pragma once

#include <QtWidgets/QWidget>
#include <QtGui/QMouseEvent>

namespace SSUtils
{
	namespace GUI
	{
		template<unsigned int MaxPositionY>
		class QDraggableFlatteningWidget : public QWidget
		{
		public:
			explicit QDraggableFlatteningWidget(QWidget *parent = nullptr)
				: QWidget(parent)
			{
				setWindowFlags(windowFlags() | Qt::FramelessWindowHint);
			}
			QDraggableFlatteningWidget(const QDraggableFlatteningWidget &ano) = delete;
			QDraggableFlatteningWidget(const QDraggableFlatteningWidget &&ano) = delete;
			QDraggableFlatteningWidget &operator=(const QDraggableFlatteningWidget &rhs) = delete;
			QDraggableFlatteningWidget &operator=(const QDraggableFlatteningWidget &&rhs) = delete;
			virtual ~QDraggableFlatteningWidget(void) = default;

		protected:
			void mouseMoveEvent(QMouseEvent * event)
			{
				if (isMousePressed == true)
				{
					QPoint movePot = event->globalPos() - mousePosition;
					move(movePot);
				}
			}

			void mousePressEvent(QMouseEvent * event)
			{
				mousePosition = event->pos();
				if (mousePosition.x() <= 0
					|| mousePosition.x() >= width()
					|| mousePosition.y() <= 0
					|| mousePosition.y() >= MaxPositionY)
				{
					return;
				}

				isMousePressed = true;
			}

			void mouseReleaseEvent(QMouseEvent * event)
			{
				isMousePressed = false;
			}

		private:
			QPoint mousePosition;
			bool isMousePressed;
		};
	};
};

#pragma once

#include <QtWidgets/QDialog>
#include <QtGui/QMouseEvent>

namespace SSUtils
{
	namespace GUI
	{
		template<unsigned int MaxPositionY>
		class QDraggableFlatteningDialog : public QDialog
		{
		public:
			enum class Result
			{
				Rejected,
				Accepted
			};

			explicit QDraggableFlatteningDialog(QWidget *parent = nullptr);
			QDraggableFlatteningDialog(const QDraggableFlatteningDialog &ano) = delete;
			QDraggableFlatteningDialog(const QDraggableFlatteningDialog &&ano) = delete;
			QDraggableFlatteningDialog &operator=(const QDraggableFlatteningDialog &rhs) = delete;
			QDraggableFlatteningDialog &operator=(const QDraggableFlatteningDialog &&rhs) = delete;
			virtual ~QDraggableFlatteningDialog(void) = default;

		protected:
			void mouseMoveEvent(QMouseEvent * event);
			void mousePressEvent(QMouseEvent * event);
			void mouseReleaseEvent(QMouseEvent * event);

		private:
			QPoint mousePosition;
			bool isMousePressed;
		};

		template<unsigned int MaxPositionY>
		QDraggableFlatteningDialog::QDraggableFlatteningDialog(QWidget * parent)
			: QDialog(parent)
		{
			setWindowFlags(windowFlags() | Qt::FramelessWindowHint);
			this->setWindowModality(Qt::WindowModal);
		}

		template<unsigned int MaxPositionY>
		void QDraggableFlatteningDialog::mouseMoveEvent(QMouseEvent * event)
		{
			if (isMousePressed == true)
			{
				QPoint movePot = event->globalPos() - mousePosition;
				move(movePot);

				if (windowState() == Qt::FullScreen)
				{
					setWindowState(Qt::NoState);
				}
			}
		}

		template<unsigned int MaxPositionY>
		void QDraggableFlatteningDialog::mousePressEvent(QMouseEvent * event)
		{
			mousePosition = event->pos();
			if (mousePosition.x() >= width()
				|| mousePosition.x() <= 0
				|| mousePosition.y() >= MaxPositionY
				|| mousePosition.y() <= 0)
			{
				return;
			}

			isMousePressed = true;
		}

		template<unsigned int MaxPositionY>
		void QDraggableFlatteningDialog::mouseReleaseEvent(QMouseEvent * event)
		{
			isMousePressed = false;
		}
	}
};
